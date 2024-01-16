using DataAccess.Daos;
using static Transversal.Utils.Log;

namespace BusinessLogic;

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
                InfoWithThread($"Battle created.");
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
                battle.Run();
            }
            Monitor.PulseAll(_lockObject);
        }
    }

    private Battle FindAvailableBattle()
    {
        return _battles.Find(b => !b.IsFull);
    }
}

