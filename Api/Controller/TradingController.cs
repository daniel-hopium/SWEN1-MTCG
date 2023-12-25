using API.HttpServer;
using Api.Utils;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Transversal.Entities;

namespace API.Controller;
public class TradingController
{
    private readonly TradingService _tradingService;
    
    public TradingController(TradingService tradingService)
    {
        _tradingService = tradingService;
    }
    
    public void ProcessRequest(object sender, HttpSvrEventArgs e)
    {
        if (e.Path.Equals("/tradings") && e.Method.Equals("GET"))
        {
            GetTrades(e);
        }
        else if (e.Path.Equals("/tradings") && e.Method.Equals("POST"))
        {
            CreateTrade(e);
        }
        else if (e.Path.StartsWith("/tradings/") && e.Method.Equals("DELETE"))
        {
            DeleteTrade(e);
        }
        else if (e.Path.StartsWith("/tradings/") && e.Method.Equals("POST"))
        {
            CarryOutTrade(e);
        }
    }

    private void CarryOutTrade(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        var tradeId = Guid.Parse(e.PathVariable());
        var cardToTrade = JsonConvert.DeserializeObject<Guid>(e.Payload);
     
        Console.WriteLine(tradeId.ToString());
        try
        {
            _tradingService.CarryOutTrade(username, tradeId, cardToTrade);
            e.Reply(200, "Trade successfully carried out");
        }
        catch
        {
            e.Reply(500, "Error carrying out trade");
        }
    }

    private void DeleteTrade(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        var tradeId = Guid.Parse(e.PathVariable());
        Console.WriteLine(tradeId.ToString());

        try
        {
            _tradingService.DeleteTrade(username, tradeId);
            e.Reply(200, "Successfully deleted trade");
        }
        catch (InvalidTradeException exception)
        {
            e.Reply(404, exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            e.Reply(403, exception.Message);
        }
        catch 
        {
            e.Reply(500, "Error deleting trade");
        }
        
    }

    private void CreateTrade(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        var trade = JsonConvert.DeserializeObject<TradeDto>(e.Payload);
        Console.WriteLine(trade!.ToString());

        try
        {
            _tradingService.CreateTrade(username, trade);
            e.Reply(200, "Successfully created trade");
        }
        catch (InvalidCardException exception)
        {
            e.Reply(403, exception.Message);
        }
        catch (InvalidTradeException exception)
        {
            e.Reply(409, exception.Message);
        }
        catch 
        {
            e.Reply(500, "Error creating trade");
        }
    }

    private void GetTrades(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        
        var tradings = _tradingService.GetTrades(username);
        if (tradings.Count == 0)
        {
            e.Reply(204, "");
            return;
        }
        e.Reply(200, JsonConvert.SerializeObject(tradings));
    }
}    


