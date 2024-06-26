﻿using Domain.ModelDTO;
using Domain.Models;
using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new SocketClient("127.0.0.1", 8000);
            bool isLoggedIn = await Login(client);
            if (isLoggedIn)
            {
                ShowMenu(client);
            }
            Console.ReadLine();
        }

        public static async Task ShowMenu(SocketClient client)
        {
            while (true)
            {
                Console.WriteLine("\nEmployee Menu:\n");
                Console.WriteLine("Choose from below options: \n1. View Menu \n2. Get List of meals rolled out by Chef \n" +
                    "3. Vote for next day meals \n4. Provide Feedback on Food Items \n5. Logout \n6. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await ViewMenu(client);
                        break;
                    case "2":
                        GetItemsRolledOutByChef(client);
                        break;
                    case "3":
                        VoteForMeals(client);
                        break;
                    case "4":
                        ProvideFeedback(client);
                        break;
                    case "5":
                        Logout(client);
                        break;
                    case "6":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Choose again, Invalid choice");
                        break;
                }
            }
        }

        private static async Task ProvideFeedback(SocketClient client)
        {
            Feedback feedback = new Feedback();
            feedback.Date = DateTime.Now;
            Console.WriteLine("\nPlease Provide your feedback");
            Console.WriteLine("\nEnter your User Id: ");
            feedback.UserId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\nEnter which item you had (Item ID): ");
            feedback.MenuItemId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\nEnter feedback message:");
            feedback.Comment = Console.ReadLine();
            Console.WriteLine("\nEnter Rating: ");
            feedback.Rating = Convert.ToInt32(Console.ReadLine());

            string jsonRequest = JsonConvert.SerializeObject(feedback);
            string request = $"ProvideFeedback_{jsonRequest}";

            var response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine($"Server response: {response}");
        }

        private async static void VoteForMeals(SocketClient client)
        {
        }

        private async static void GetItemsRolledOutByChef(SocketClient client)
        {
            string request = "GetRolledOutItems";
            string response = await client.CommunicateWithStreamAsync(request);
            var deserializedResponse = JsonConvert.DeserializeObject(response);
            Console.WriteLine($"Rolled Out Menu: {deserializedResponse}");
        }

        public static async Task<bool> Login(SocketClient client)
        {
            UserDTO user = new UserDTO();

            Console.WriteLine("Enter your User Id: ");
            user.Id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter your User name: ");
            user.Name = Console.ReadLine();

            string jsonRequest = JsonConvert.SerializeObject(user);
            string request = $"EmployeeLogin_{jsonRequest}";

            string response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine($"Server response: {response}");

            return response.Contains("Login");
        }
        public static async Task ViewMenu(SocketClient client)
        {
            string request = "ViewMenu";
            string response = await client.CommunicateWithStreamAsync(request);
            var deserializedResponse = JsonConvert.DeserializeObject(response);
            Console.WriteLine($"Menu: {deserializedResponse}");
        }
        static void Logout(SocketClient client)
        {
            Console.WriteLine("User Logged out, Please Login again to continue");
            Login(client);
        }
    }
}
