using System.Text.Json;
using API.HttpServer;
using Api.Utils;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Transversal.Entities;


namespace API.Controller
{
    public class UserController
    {
        private UserService _userService = new UserService();
        private Utils _utils = new Utils();
        
        public void ProcessRequest(object sender, HttpSvrEventArgs e)
        {

            if (e.Path.Equals("/users") && e.Method.Equals("POST"))
            {
                CreateUser(e);
            }
            else if (e.Path.StartsWith("/users/") && e.Method.Equals("GET"))
            {
                GetUserData(e);
            }
            else if (e.Path.StartsWith("/users/") && e.Method.Equals("PUT"))
            {
                UpdateUserData(e);
            }
            else if (e.Path.Equals("/sessions") && e.Method.Equals("POST"))
            {
                Login(e);
            }
        }

        private void Login(HttpSvrEventArgs httpSvrEventArgs)
        {
            throw new NotImplementedException();
        }

        private void UpdateUserData(HttpSvrEventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(e.Payload);
            user.Username = _utils.PathVariable(e.Path);
            
            // JsonDocument jsonDocument = JsonDocument.Parse(e.Payload);
            // JsonElement root = jsonDocument.RootElement;
            //
            // user.username = pathVariable;
            // user.name = root.GetProperty("name").GetString(); // FIX NULLABLE
            // user.bio = root.GetProperty("bio").GetString();
            // user.image = root.GetProperty("image").GetString();

            var updatedUser = _userService.UpdateUser(user);
            
            e.Reply(200, JsonConvert.SerializeObject(updatedUser));
        }

        private void GetUserData(HttpSvrEventArgs e)
        {
            string username = _utils.PathVariable(e.Path);

            var updatedUser = _userService.GetData(username);
            e.Reply(200, JsonConvert.SerializeObject(updatedUser));
        }

        private void CreateUser(HttpSvrEventArgs e)
        {
            
            JsonDocument jsonDocument = JsonDocument.Parse(e.Payload);
            JsonElement root = jsonDocument.RootElement;
            
            User user = new User();
            
            Console.WriteLine(e.Payload);
            user.Username = root.GetProperty("username").GetString();
            user.Password = root.GetProperty("password").GetString();
            try
            {
                _userService.CreateUser(user);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                e.Reply(400, "Username already exists");
                return;
            }
            e.Reply(201, "User successfully created");
        }
        
    }
}

