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

        private void Login(HttpSvrEventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(e.Payload);

            if (_userService.Login(user))
            {
                e.Reply(200, "Successfully logged in");
            }
            else
            {
                e.Reply(400, "Username or Password is wrong");
            }
        }

        private void UpdateUserData(HttpSvrEventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(e.Payload);
            user!.Username = e.PathVariable();
            
            if (!_userService.UserExists(user.Username))
            {
                e.Reply(400, "User not found");
                return;
            }
            
            var updatedUser = _userService.UpdateUser(user);
            e.Reply(200, JsonConvert.SerializeObject(updatedUser));
        }

        private void GetUserData(HttpSvrEventArgs e)
        {
            var updatedUser = _userService.GetUser(e.PathVariable());
            
            if (updatedUser.Username == null)
                e.Reply(400, "User not found");
            else
                e.Reply(200, JsonConvert.SerializeObject(updatedUser));
        }

        private void CreateUser(HttpSvrEventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(e.Payload);
            Console.WriteLine(user);
            // if (_userService.UserExists(user.Username))
            // {
            //     e.Reply(400, "Username already taken");
            //     return;
            // }
            
            _userService.CreateUser(user);
            e.Reply(201, "User successfully created");
        }
        
    }
}

