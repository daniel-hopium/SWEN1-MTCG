using DataAccess.Daos;

namespace BusinessLogic.Utils;

    class BattleManager
    {
        private List<Battle> battles = new List<Battle>();
        private object lockObject = new object();

        public void JoinBattle(UserDao player)
        {
            lock (lockObject)
            {
                Battle battle = FindAvailableBattle();

                if (battle == null)
                {
                    // If no available battles, create a new one
                    battle = new Battle();
                    battles.Add(battle);
                    Log($"Battle {battle.Id} created.");
                }

                // Join the battle
                battle.Join(player);

                // If the battle is full, signal the event to start the battle
                while (!battle.IsFull)
                {
                    Monitor.Wait(lockObject);
                }
                Monitor.PulseAll(lockObject);
                Console.WriteLine(player.Username + " started into battle " + battle.Id);
                battle.EnterBattle();

            }
        }

        private Battle FindAvailableBattle()
        {
            return battles.Find(b => !b.IsFull);
        }

        private static void Log(string message)
        {
            Console.WriteLine($"Thread-{Thread.CurrentThread.ManagedThreadId}: {message}");
        }

        class Battle
    {
        private object lockObject = new object();
        private List<UserDao> players = new List<UserDao>();
        private List<string> battleLog = new List<string>();
        private static int battleCount = 0;
        private bool isRunning = true;
        private static bool firstPlayerTurn = true;

        public int Id { get; }

        public Battle()
        {
            lock (lockObject)
            {
                Id = ++battleCount;
            }
        }

        public void Join(UserDao player)
        {
            lock (lockObject)
            {
                players.Add(player);
                Log($"{player.Username} joined Battle {Id}.");
            }
        }

        public void EnterBattle()
        {

            while (battleCount <= 10)
            {
                Console.WriteLine("Battle is running....");
                Console.WriteLine($"{players[0].Username} vs {players[1].Username}");
                
                
                // Simulate a round of battle
                Monitor.Enter(lockObject);
                if (firstPlayerTurn)
                {
                    SimulateBattleRound(players[0], players[1]);
                    battleCount++;
                }
                else
                {
                    SimulateBattleRound(players[1], players[0]);
                    battleCount++;
                }
                firstPlayerTurn = !firstPlayerTurn;
                Monitor.Exit(lockObject);

            }
            
            // Capture battle details in the log
            CaptureBattleLog();

            //  Remove players from the battle when it's completed
            //  ResetBattle();
            

            Console.WriteLine($"Battle {Id} completed.");
        }

        private void SimulateBattleRound(UserDao attacker, UserDao defender)
        {
            // Simulate battle logic for one round
            // For simplicity, just print the attack information

            int damage = 10; // Simulated damage value

            Log($"{attacker.Username} attacks {defender.Username} and deals {damage} damage.");

            // Update player health or other relevant information
            // ...

            // Simulate some delay between rounds
            Thread.Sleep(1000);
        }

        private void CaptureBattleLog()
        {
            lock (lockObject)
            {
                battleLog.Add($"Battle {Id} details:");

                // Add player-specific details to the log
                foreach (var player in players)
                {
                    battleLog.Add($"- {player.Username}");
                }

                // Add other battle details
                // ...

                Log($"Battle {Id} completed.");
            }
        }

        private void Log(string message)
        {
            Console.WriteLine($"Thread-{Thread.CurrentThread.ManagedThreadId}: {message}");
            
        }

        public bool IsFull => players.Count == 2;
        
    }
}

