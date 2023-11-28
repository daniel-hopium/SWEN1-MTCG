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
            svr.Incoming += _ProcessMesage;

            svr.Run();
        }


        /// <summary>Event handler for incoming server requests.</summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private static void _ProcessMesage(object sender, HttpSvrEventArgs e)
        {
            
            //if startswith ...........

            if (e.Path.StartsWith("/users") || e.Path.StartsWith("/sessions"))
            {
                UserController userController = new UserController();
                userController.ProcessRequest(sender, e);

            }
            else if (e.Path.StartsWith("/packages") || e.Path.StartsWith("/transactions"))
            {
            
            }
            else if (e.Path.StartsWith("/cards") || e.Path.StartsWith("/deck"))
            {
                
            }
            else if (e.Path.StartsWith("/stats") || e.Path.StartsWith("/scoreboard")|| e.Path.StartsWith("/battles"))
            {
                
            }
            else if (e.Path.StartsWith("/tradings"))
            {
                
            }
            
            /*switch (e.Path)
            {
                case "/":
                    e.Reply(200, "Hello World!");
                    return;
                case "/cards":
                    e.Reply(400, "CardsValue");
                    return;
                default:
                    e.Reply(200, "Yo! Understood.");
                    break;
            }*/
        }
    }
}