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
        public static async Task Main(string[] args)
        {
            var client = new SocketClient("127.0.0.1", 8000);
            bool isLoggedIn = await Login(client);
            if(isLoggedIn)
            {
                ShowMenu(client);
            }
            Console.ReadLine();
        }

        public static async Task<bool> Login(SocketClient client)
        {
            UserDTO user = new UserDTO();

            Console.WriteLine("Enter your User Id: ");
            user.Id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter your User name: ");
            user.Name = Console.ReadLine();

            string jsonRequest = JsonConvert.SerializeObject(user);
            string request = $"ChefLogin_{jsonRequest}";

            string response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine($"Server response: {response}");

            return response.Contains("Login");
        }
        public static async Task ShowMenu(SocketClient client)
        {
            while (true)
            {
                Console.WriteLine("\nChef Menu:\n");
                Console.WriteLine("Choose from below options: \n1. View Menu \n2. Get List of recommended meals \n" +
                    "3. Roll Out Food items \n4. Logout \n5. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await ViewMenu(client);
                        break;
                    case "2":
                        GetRecommmendedItems(client);
                        break;
                    case "3":
                        RollOutItems(client);
                        break;
                    case "4":
                        Logout(client);
                        break;
                    case "5":
                        ViewAllFeedbacks(client);
                        break;
                    case "6":
                        ViewFeedbacksById(client);
                        break;
                    case "7":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Choose again, Invalid choice");
                        break;
                }
            }
        }

        private static void ViewAllFeedbacks(SocketClient client)
        {
            throw new NotImplementedException();
        }

        private static void ViewFeedbacksById(SocketClient client)
        {
            throw new NotImplementedException();
        }

        public static async Task ViewMenu(SocketClient client)
        {
            string request = "ViewMenu";
            string response = await client.CommunicateWithStreamAsync(request);
            var deserializedResponse = JsonConvert.DeserializeObject(response);
            Console.WriteLine($"Menu: {deserializedResponse}");
        }
        public static void GetRecommmendedItems(SocketClient client)
        {

        }
        public static void RollOutItems(SocketClient client)
        {

        }
        static void Logout(SocketClient client)
        {
            Console.WriteLine("User Logged out, Please Login again to continue");
            Login(client);
        }
    }
}
