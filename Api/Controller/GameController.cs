using API.HttpServer;
using Api.Utils;
using BusinessLogic.Services;
using Newtonsoft.Json;

namespace API.Controller;
public class GameController
{
    private readonly GameService _gameService;
    
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
            StartBattle(e);
        }
    }

    private void GetStats(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        var user = _gameService.GetUserStats(username);
        e.Reply(200, JsonConvert.SerializeObject(user));
    }
    
    private void GetScoreboard(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }
        var scoreboard = _gameService.GetScoreboard();
        e.Reply(200, JsonConvert.SerializeObject(scoreboard));
    }
    
    private void StartBattle(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        try
        {
            var battleStats = _gameService.StartBattle(username);
            e.Reply(200, JsonConvert.SerializeObject(battleStats));
        }
        catch (Exception exception)
        {
            e.Reply(500, "Internal Server Error");
        }
    }
}


