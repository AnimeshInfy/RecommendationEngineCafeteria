using Domain.ModelDTO;
using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new SocketClient("127.0.0.1", 8000);
            Login(client);
            Console.ReadLine();
        }

        public static async Task<bool> Login(SocketClient client)
        {
            bool loginStatus = false;
            UserDTO user = new UserDTO();

            Console.WriteLine("Enter your User Id: ");
            user.Id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter your User name: ");
            user.Name = Console.ReadLine();

            string jsonRequest = JsonConvert.SerializeObject(user);
            string request = $"ChefLogin_{jsonRequest}";

            client.SendMessage(request);

            string response = client.ReceiveMessage();
            Console.WriteLine($"Server response: {response}");

            loginStatus = response.Contains("ChefLogin");

            return loginStatus;
        }
    }
}
