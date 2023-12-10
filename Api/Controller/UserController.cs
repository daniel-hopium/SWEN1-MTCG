using API.HttpServer;
using Api.Utils;
using BusinessLogic.Services;
using DataAccess.Repository;
using Newtonsoft.Json;
using Transversal.Entities;

namespace API.Controller
{
    public class UserController
    {
        private UserService _userService;
        private Utils _utils = new Utils();
        
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
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
           var user = JsonConvert.DeserializeObject<User>(e.Payload);

            if (_userService.Login(user))
            {
                e.Reply(200, $"{user.Username}-mtcgToken");
            }
            else
            {
                e.Reply(401, "Invalid username or password provided");
            }
        }

        private void UpdateUserData(HttpSvrEventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(e.Payload);
            user!.Username = e.PathVariable();
            
            if (!_userService.UserExists(user.Username))
            {
                e.Reply(404, "User not found");
                return;
            }
            
            var updatedUser = _userService.UpdateUser(user);
            e.Reply(200, JsonConvert.SerializeObject(updatedUser));
        }

        private void GetUserData(HttpSvrEventArgs e)
        {
            if (!_userService.UserExists(e.PathVariable()))
            { 
                e.Reply(404, "User not found");
                return; 
            }
            
            var updatedUser = _userService.GetUser(e.PathVariable());
            e.Reply(200, JsonConvert.SerializeObject(updatedUser));
        }

        private void CreateUser(HttpSvrEventArgs e)
        {
            var user = JsonConvert.DeserializeObject<User>(e.Payload);
            
            if (user == null) 
            {
                e.Reply(400, "Invalid JSON format");
                return;
            }
            
            if (_userService.UserExists(user.Username))
            {
                e.Reply(409, "User with same username already registered");
                return;
            }
            
            _userService.CreateUser(user);
            e.Reply(201, "User successfully created");
        }
        
    }
}

