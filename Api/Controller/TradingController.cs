using API.HttpServer;

namespace API.Controller
{
    public class TradingController
    {
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

        private void CreateTrade(HttpSvrEventArgs httpSvrEventArgs)
        {
            throw new NotImplementedException();
        }

        private void DeleteTrading(HttpSvrEventArgs httpSvrEventArgs)
        {
            throw new NotImplementedException();
        }

        private void CreateTrading(HttpSvrEventArgs httpSvrEventArgs)
        {
            throw new NotImplementedException();
        }

        private void GetTradings(HttpSvrEventArgs httpSvrEventArgs)
        {
            throw new NotImplementedException();
        }
    }    
}

