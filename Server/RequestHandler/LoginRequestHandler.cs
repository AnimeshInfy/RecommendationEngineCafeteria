using Domain.Services;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.IServices;
using System.Text.Json.Serialization;
using Domain.ModelDTO;
using Newtonsoft.Json;

namespace Server.RequestHandler {
    public class LoginRequestHandler
    {
        private readonly IUserService _userService;

        public LoginRequestHandler(IUserService userService)
        {
           _userService = userService;
        }

        public async Task<string> HandleRequestAsync(string request)
        {
            if (request.Contains("Login"))
            {
                var a = request.Split('_');
                var jsonDeserialized = JsonConvert.DeserializeObject<UserDTO>(a[1]);
                var user = await _userService.AuthenticateUserAsync(jsonDeserialized.Id, jsonDeserialized.Name);
                if (user != null)
                {
                    return "Logged in successfully";
                }
                return "Invalid Credentials";
            }
            return null; //TEMPORARY I HAVE TO CHANGE IT
        }
    }
}

