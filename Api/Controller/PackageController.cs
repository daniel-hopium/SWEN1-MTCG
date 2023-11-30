using API.HttpServer;

namespace API.Controller
{
    public class PackageController
    {
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

