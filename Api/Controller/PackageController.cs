using API.HttpServer;
using Api.Models;
using Api.Utils;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Transversal.Entities;

namespace API.Controller
{
    public class PackageController
    {
        private PackageService _packageService;
        
        public PackageController(PackageService packageService)
        {
            _packageService = packageService;
        }
        
        public void ProcessRequest(object sender, HttpSvrEventArgs e)
        {

            if (e.Path.Equals("/packages") && e.Method.Equals("POST"))
            {
                CreatePackages(e);
            }
            else if (e.Path.Equals("/transactions/packages") && e.Method.Equals("POST"))
            {
                BuyCardPackage(e);
            }
        }

        private void BuyCardPackage(HttpSvrEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CreatePackages(HttpSvrEventArgs e)
        {
            if(Authorization.AuthorizeAdmin(e.Authorization))
            {
                var cards = JsonConvert.DeserializeObject<List<CardsDto>>(e.Payload);
                _packageService.CreatePackage(cards);
                e.Reply(200, "Package created");
            }
            else
            {
                e.Reply(401, "Unauthorized");
            }
            Console.WriteLine(e.PlainMessage);
            e.Reply(200, "wda");
        }
    }   
}

