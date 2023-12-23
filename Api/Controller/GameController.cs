using API.HttpServer;
using Api.Utils;
using BusinessLogic.Services;

namespace API.Controller
{
    public class GameController
    {
        private GameService _gameService;
        
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }
        
        public void ProcessRequest(object sender, HttpSvrEventArgs e)
        {
            if (e.Path.Equals("/stats") && e.Method.Equals("GET"))
            {
                GetStats(e);
            }
            else if (e.Path.Equals("/scoreboard") && e.Method.Equals("GET"))
            {
                GetScoreboard(e);
            }
            else if (e.Path.Equals("/battles") && e.Method.Equals("POST"))
            {
                AttemptStartBattle(e);
            }
        
        }

        private void AttemptStartBattle(HttpSvrEventArgs e)
        {
            if (!Authorization.AuthorizeUser(e.Authorization))
            {
                e.Reply(401, "Unauthorized");
                return;
            }
            var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
            var battleStats = _gameService.AttemptStartBattle(username);
            e.Reply(200, battleStats);
        }

        private void GetScoreboard(HttpSvrEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GetStats(HttpSvrEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

