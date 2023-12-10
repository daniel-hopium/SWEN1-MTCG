using API.HttpServer;
using BusinessLogic.Services;

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

        private void BuyCardPackage(HttpSvrEventArgs httpSvrEventArgs)
        {
            throw new NotImplementedException();
        }

        private void CreatePackages(HttpSvrEventArgs httpSvrEventArgs)
        {
            throw new NotImplementedException();
        }
    }   
}

