using API.HttpServer;
using BusinessLogic.Services;

namespace API.Controller
{
    public class CardsController
    {
        private CardsService _cardsService;
        
        public CardsController(CardsService cardsService)
        {
            _cardsService = cardsService;
        }
        
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

        private void ConfigureDeck(HttpSvrEventArgs e)
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
}