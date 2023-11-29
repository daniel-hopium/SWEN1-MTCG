namespace TradingCardGame.NET.Controller;

public class GameController
{
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

    private void AttemptStartBattle(HttpSvrEventArgs httpSvrEventArgs)
    {
        throw new NotImplementedException();
    }

    private void GetScoreboard(HttpSvrEventArgs httpSvrEventArgs)
    {
        throw new NotImplementedException();
    }

    private void GetStats(HttpSvrEventArgs e)
    {
        throw new NotImplementedException();
    }
}