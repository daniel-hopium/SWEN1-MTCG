using TradingCardGame.NET.Controller;

namespace TradingCardGame.NET
{
    internal class Program
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // entry point                                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Main entry point.</summary>
        /// <param name="args">Arguments.</param>
        static void Main(string[] args)
        {
            HttpSvr svr = new HttpSvr();
            DatabaseManager.OpenDatabaseConnection();
            svr.Incoming += _ProcessMesage;

            svr.Run();
            DatabaseManager.CloseDatabaseConnection();
        }


        /// <summary>Event handler for incoming server requests.</summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private static void _ProcessMesage(object sender, HttpSvrEventArgs e)
        {
            
            if (e.Path.StartsWith("/users") || e.Path.StartsWith("/sessions"))
            {
                UserController userController = new UserController();
                userController.ProcessRequest(sender, e);

            }
            else if (e.Path.StartsWith("/packages") || e.Path.StartsWith("/transactions"))
            {
                PackageController packageController = new PackageController();
                packageController.ProcessRequest(sender, e);
            }
            else if (e.Path.StartsWith("/cards") || e.Path.StartsWith("/deck"))
            {
                CardsController cardsController = new CardsController();
                cardsController.ProcessRequest(sender, e);

            }
            else if (e.Path.StartsWith("/stats") || e.Path.StartsWith("/scoreboard")|| e.Path.StartsWith("/battles"))
            {
                GameController gameController = new GameController();
                gameController.ProcessRequest(sender, e);
            }
            else if (e.Path.StartsWith("/tradings"))
            {
                TradingController tradingController = new TradingController();
                tradingController.ProcessRequest(sender, e);
                
            }
        }
    }
}