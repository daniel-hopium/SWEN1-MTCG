using DataAccess.Daos;
using DataAccess.Repository;
using static BusinessLogic.Utils.Effectiveness;
using static Transversal.Utils.Logger;

namespace BusinessLogic.Utils;

public enum Effectiveness
{
    Effective,
    NotEffective,
    Normal
}

public enum Effects
{
    None,
    Afraid,
    Controlled,
    Drowned,
    Immune,
    Evade
}

class BattleManager
{
    private readonly List<Battle> _battles = new List<Battle>();
    private readonly object _lockObject = new object();

    public void JoinBattle(UserDao player)
    {
        lock (_lockObject)
        {
            Battle battle = FindAvailableBattle();

            if (battle == null)
            {
                // If no available battles, create a new one
                battle = new Battle();
                _battles.Add(battle);
                Log($"Battle created.");
            }

            // Join the battle
            battle.Join(player);

            // If the battle is full, signal the event to start the battle
            if (!battle.IsFull)
            {
                Monitor.Wait(_lockObject);
            }
            else
            {
                Console.WriteLine(player.Username + " started into battle " );
                battle.EnterBattle();
            }
            Monitor.PulseAll(_lockObject);
        }
    }

    private Battle FindAvailableBattle()
    {
        return _battles.Find(b => !b.IsFull);
    }

    private static void Log(string message)
    {
        Console.WriteLine($"Thread-{Thread.CurrentThread.ManagedThreadId}: {message}");
    }

    class Battle
    {
        private readonly GameRepository _gameRepository = new GameRepository();
        private readonly UserRepository _userRepository = new UserRepository();
        
        private readonly object _lockObject = new object();
        private readonly List<UserDao> _players = new List<UserDao>();
        private readonly List<short> _playerWins = Enumerable.Repeat((short)0, 2).ToList(); 
        private readonly List<string> _battleLog = new List<string>();
        private static int _battleCount = 0;
        private readonly int _maxRounds = 10;

        public void Join(UserDao player)
        {
            lock (_lockObject)
            {
                _players.Add(player);
                Log($"{player.Username} joined Battle.");
            }
        }

        public void EnterBattle()
        {
            
            Console.WriteLine("Battle is starting....");
            Console.WriteLine($"PlayerA:{_players[0].Username} vs PlayerB:{_players[1].Username}");
        
            try
            {
                while (_battleCount <= _maxRounds)
                {
                    SimulateBattleRound();
                    _battleCount++;
                }
                LogResult();
                
                PersistBattleLog();
                UpdatePlayerStats();
                
                ResetBattle();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ResetBattle();
            }
            
            Console.WriteLine($"Battle completed.");
        }

        private void LogResult()
        {
            string log = $"PlayerA: {_players[0].Username} wins {_playerWins[0]} rounds. PlayerB: {_players[1].Username} wins {_playerWins[1]} rounds. Draws: {_battleCount - _playerWins[0] - _playerWins[1]}";
            LogInfo(log);
            _battleLog.Add(log);
        }

        private void UpdatePlayerStats()
        {
            _userRepository.UpdateStats(_players[0], _playerWins[0] > _playerWins[1]);
            _userRepository.UpdateStats(_players[1], _playerWins[1] > _playerWins[0]);
            
        }
        
        private void PersistBattleLog()
        {
            _gameRepository.SaveBattle(new BattleDao()
            {
                WinnerId = _playerWins[0] > _playerWins[1] ? _players[0].Id : _players[1].Id,
                OpponentId = _playerWins[0] > _playerWins[1] ? _players[1].Id : _players[0].Id,
                Log = string.Join("\n", _battleLog),
            });
        }

        private void ResetBattle()
        {
            _players.Clear();
            _playerWins[0] = 0;
            _playerWins[1] = 0;
            _battleLog.Clear();
            _battleCount = 0;
        }

        private void SimulateBattleRound()
        {
            // take random cards from each deck
            CardDao playerACard = _players[0].Deck![new Random().Next(_players[0].Deck!.Count)];
            CardDao playerBCard = _players[1].Deck![new Random().Next(_players[1].Deck!.Count)];
            
            Fight(playerACard, playerBCard);
            
            // Simulate some delay between rounds
            Thread.Sleep(1000);
        }

        private void Fight(CardDao playerACard, CardDao playerBCard)
        {
            double actualPlayerACardDamage = CalculateDamage(playerACard, playerBCard);
            double actualPlayerBCardDamage = CalculateDamage(playerBCard, playerACard);
            // Effects effectPlayerA = CheckEffect(playerACard, playerBCard);
            // Effects effectPlayerB = CheckEffect(playerBCard, playerACard);
            
                string log =
                    $"Round {_battleCount}: PlayerA: {playerACard.Name}({playerACard.Damage}) vs PlayerB: {playerBCard.Name}({playerBCard.Damage}) => " +
                    $"{playerACard.Damage} vs {playerACard.Damage} => {actualPlayerACardDamage} vs {actualPlayerBCardDamage} => ";
                if (actualPlayerACardDamage > actualPlayerBCardDamage)
                {
                    log += playerACard.Name + " wins";
                    _playerWins[0] += 1;
                }
                else if (actualPlayerACardDamage < actualPlayerBCardDamage)
                {
                    log += playerBCard.Name + " wins";
                    _playerWins[1] += 1;
                }
                else
                {
                    log += "Draw";
                }
                
                LogInfo(log);
                _battleLog.Add(log);
        }

        private Effects CheckEffect(CardDao actualCard, CardDao comparisonCard)
        {
            Effects effect = Effects.None;
            switch (actualCard.CardType)
            {
                case "goblin":
                    if (comparisonCard.CardType == "dragon")
                        effect = Effects.Afraid;
                    break;
                case "wizzard":
                    if (comparisonCard.CardType == "ork")
                        effect = Effects.Controlled;
                    break;
                case "knight":
                    if (comparisonCard.CardType == "spell" && comparisonCard.ElementType == "water")
                        effect = Effects.Drowned;
                    break;
                case "kraken":
                    if (comparisonCard.CardType == "spell")
                        effect = Effects.Immune;
                    break;
                case "elve":
                    if (actualCard.ElementType == "fire" && comparisonCard.CardType == "dragon")
                        effect = Effects.Evade;
                    break;
                default:
                    effect = Effects.None;
                    break;
            }

            return effect;
        }

        private double CalculateDamage(CardDao actualCard, CardDao cardToCompare)
        {
            var damage = 0.0;

            Effectiveness effectiveness = Normal;

            switch (actualCard.ElementType)
            {
                case "fire" when cardToCompare.ElementType == "water":
                    effectiveness = NotEffective;
                    break;
                case "fire" when cardToCompare.ElementType == "regular":
                    effectiveness = Effective;
                    break;
                case "water" when cardToCompare.ElementType == "fire":
                    effectiveness = NotEffective;
                    break;
                case "water" when cardToCompare.ElementType == "regular":
                    effectiveness = NotEffective;
                    break;
                case "regular" when cardToCompare.ElementType == "water":
                    effectiveness = Normal;
                    break;
                case "regular" when cardToCompare.ElementType == "fire":
                    effectiveness = Normal;
                    break;
                case "regular" when cardToCompare.ElementType == "regular":
                    effectiveness = Normal;
                    break;
                default:
                    damage = actualCard.Damage;
                    break;
            }
            
            switch (effectiveness)
            {
                case Effective:
                    damage = actualCard.Damage * 2;
                    break;
                case NotEffective:
                    damage = actualCard.Damage / 2;
                    break;
                case Normal:
                    damage = actualCard.Damage;
                    break;
            }

            return damage;
        }
        
        private void Log(string message)
        {
            Console.WriteLine($"Thread-{Thread.CurrentThread.ManagedThreadId}: {message}");
        }

        public bool IsFull => _players.Count == 2;
    }
}

