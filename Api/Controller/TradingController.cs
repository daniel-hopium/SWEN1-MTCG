using API.HttpServer;
using Api.Utils;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Transversal.Entities;

namespace API.Controller
{
    public class TradingController
    {
        private TradingService _tradingService;
        
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
            throw new NotImplementedException();
        }

        private void DeleteTrade(HttpSvrEventArgs e)
        {
            throw new NotImplementedException();
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
            Console.WriteLine(trade.ToString());
            
            _tradingService.CreateTrade(username, trade);
            
            
            e.Reply(200, "Successfully created trade");
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
}

