using API.HttpServer;
using Api.Utils;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Transversal.Entities;

namespace API.Controller
{
    public class PackageController
    {
        private readonly PackageService _packageService;
        
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
            if (!Authorization.AuthorizeUser(e.Authorization))
            {
                e.Reply(401, "Unauthorized");
                return;
            }

            var package = JsonConvert.DeserializeObject<PackageDto>(e.Payload);
            var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
            try
            {
                _packageService.BuyCardPackage(package, username);
                e.Reply(201, "Package bought");
            }
            catch (Exception exception)
            {
                e.Reply(400, exception.Message);
            }
        }

        private void CreatePackages(HttpSvrEventArgs e)
        {
            if (!Authorization.AuthorizeAdmin(e.Authorization))
            {
                e.Reply(401, "Unauthorized");
                return;
            }
            
            var cards = JsonConvert.DeserializeObject<List<CardDto>>(e.Payload);
            _packageService.CreatePackage(cards!);
            e.Reply(201, "Package and cards successfully created");
            
        }
    }   
}

