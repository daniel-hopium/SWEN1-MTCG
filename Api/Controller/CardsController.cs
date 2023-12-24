using System.Web;
using API.HttpServer;
using Api.Utils;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Transversal.Entities;

namespace API.Controller
{
    public class CardsController
    {
        private readonly CardsService _cardsService;
        
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
            else if (e.Path.StartsWith("/deck") && e.Method.Equals("GET"))
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
            if (!Authorization.AuthorizeUser(e.Authorization))
            {
                e.Reply(401, "Unauthorized");
                return;
            }
            var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
            var cardIds = JsonConvert.DeserializeObject<List<Guid>>(e.Payload);
            Console.WriteLine(cardIds);
            var cards = cardIds!.Select(cardId => new CardDto { Id = cardId }).ToList();
            try
            {
                _cardsService.ConfigureDeck(username, cards);
                e.Reply(200, "Successfully configured deck");
            }
            catch (Exception exception)
            {
                e.Reply(400, exception.Message);
            }
            
        }

        private void GetDeck(HttpSvrEventArgs e)
        {
            if (!Authorization.AuthorizeUser(e.Authorization))
            {
                e.Reply(401, "Unauthorized");
                return;
            }
            var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
            
            var cards = _cardsService.GetDeck(username);
            if (cards.Count == 0)
            {
                e.Reply(204, "");
                return;
            }

            if (HttpUtility.ParseQueryString(e.Path).Get("format") == "plain")
                e.Reply(200, string.Join(", ", cards.Select(card => card.Id)));
            else
                e.Reply(200, JsonConvert.SerializeObject(cards));
        }

        private void GetAllCards(HttpSvrEventArgs e)
        {
            if (!Authorization.AuthorizeUser(e.Authorization))
            {
                e.Reply(401, "Unauthorized");
                return;
            }
            var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
            
            var cards = _cardsService.GetAllCards(username);
            if (cards.Count == 0)
            {
                e.Reply(204, "");
                return;
            }
            e.Reply(200, JsonConvert.SerializeObject(cards));
        }
    }
}