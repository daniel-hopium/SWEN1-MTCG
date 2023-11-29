namespace TradingCardGame.NET.Controller;

public class CardsController
{
    public void ProcessRequest(object sender, HttpSvrEventArgs e)
    {
        if (e.Path.Equals("/cards") && e.Method.Equals("GET"))
        {
            GetAllCards(e);
        }
        else if (e.Path.Equals("/deck") && e.Method.Equals("GET"))
        {
            GetDeck(e);
        }
        else if (e.Path.Equals("/deck") && e.Method.Equals("PUT"))
        {
            ConfigureDeck(e);
        }
    }

    private void ConfigureDeck(HttpSvrEventArgs httpSvrEventArgs)
    {
        throw new NotImplementedException();
    }

    private void GetDeck(HttpSvrEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void GetAllCards(HttpSvrEventArgs e)
    {
        throw new NotImplementedException();
    }
}