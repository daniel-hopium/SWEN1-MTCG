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
                GetTradings(e);
            }
            else if (e.Path.Equals("/tradings") && e.Method.Equals("POST"))
            {
                CreateTrading(e);
            }
            else if (e.Path.StartsWith("/tradings/") && e.Method.Equals("DELETE"))
            {
                DeleteTrading(e);
            }
            else if (e.Path.StartsWith("/tradings/") && e.Method.Equals("POST"))
            {
                CreateTrade(e);
            }
        
        }

        private void CreateTrade(HttpSvrEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteTrading(HttpSvrEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CreateTrading(HttpSvrEventArgs e)
        {
            if (!Authorization.AuthorizeUser(e.Authorization))
            {
                e.Reply(401, "Unauthorized");
                return;
            }
            var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
            var trade = JsonConvert.DeserializeObject<TradeDto>(e.Payload);
            Console.WriteLine(trade);
            
            _tradingService.CreateTrading(username, trade);
            
            
            e.Reply(200, "Successfully created trade");
        }

        private void GetTradings(HttpSvrEventArgs e)
        {
            if (!Authorization.AuthorizeUser(e.Authorization))
            {
                e.Reply(401, "Unauthorized");
                return;
            }
            var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
            
            var tradings = _tradingService.GetTradings(username);
            if (tradings.Count == 0)
            {
                e.Reply(204, "");
                return;
            }
            e.Reply(200, JsonConvert.SerializeObject(tradings));
        }
    }    
}

