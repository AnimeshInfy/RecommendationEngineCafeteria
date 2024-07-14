using Domain.ModelDTO;
using Domain.Models;
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
                Console.WriteLine("Choose from below options: \n1. View Menu " +
                    "\n2. Get List of recommended meals \n3. Roll Out Food items " +
                    "\n4. Logout \n5. View Feedbacks \n6. View Feedback by food id" +
                    "\n7. View Max voted items \n8. Send Notifications " +
                    "\n9. Take actions on Discarded items \n10. Exit");
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
                        //View max voted items
                        break;
                    case "8":
                        SendNotifications(client);
                        break;
                    case "9":
                        TakeActionOnDiscardedMenu(client);
                        break;
                    case "10":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Choose again, Invalid choice");
                        break;
                }
            }
        }

        private static async void ViewAllFeedbacks(SocketClient client)
        {
            string request = "ViewFeedbacks";
            string response = await client.CommunicateWithStreamAsync(request);
            var deserializedResponse = JsonConvert.DeserializeObject(response);
            Console.WriteLine($"Feedbacks: {deserializedResponse}");
        }

        private static async void ViewFeedbacksById(SocketClient client)
        {
            Console.WriteLine("Enter the food id for which you want to see the feedback");
            int foodId = Convert.ToInt32(Console.ReadLine());
            string request = $"ViewFeedbacksById_{foodId}";
            string response = await client.CommunicateWithStreamAsync(request);
            var deserializedResponse = JsonConvert.DeserializeObject(response);
            Console.WriteLine($"Feedbacks: {deserializedResponse}");
        }

        public static async Task ViewMenu(SocketClient client)
        {
            string request = "ViewMenu";
            string response = await client.CommunicateWithStreamAsync(request);
            var deserializedResponse = JsonConvert.DeserializeObject(response);
            Console.WriteLine($"Menu: {deserializedResponse}");
        }
        public static async void GetRecommmendedItems(SocketClient client)
        {
            Console.WriteLine("\nEnter the number of items you want to fetch from Recommnendation Engine:");
            var noOfRecommendedItems = Console.ReadLine();
            string request = $"GetRecommendedMeals_{noOfRecommendedItems}";
            string response = await client.CommunicateWithStreamAsync(request);   
            var deserializedResponse = JsonConvert.DeserializeObject(response);
            Console.WriteLine($"Top Recommmended Meals: {deserializedResponse}");
        }
        public static async void RollOutItems(SocketClient client)
        {
            Console.WriteLine("\nEnter the Item Id(s) which you want to roll out for next day:");
            string itemIds = Console.ReadLine();
            string[] itemIdsArray = itemIds.Split(',');
            List<int> itemIdsInt = itemIdsArray.Select(int.Parse).ToList();

            string request = $"RollOutItems_{itemIds}";
            string response = await client.CommunicateWithStreamAsync(request);
        }
        static void Logout(SocketClient client)
        {
            Console.WriteLine("User Logged out, Please Login again to continue");
            Login(client);
        }
        public static async void SendNotifications(SocketClient client)
        {
            Console.WriteLine("\nPlease write the message you want to convey to Users\n");
            string message = Console.ReadLine();
            Console.WriteLine("Enter the target users for receving the notification: \n");
            Console.WriteLine("If you want to send notifications to all users, press Y\n");
            string sendToAllUsers = Console.ReadLine();
            string request = "";
            if (sendToAllUsers.ToLower() == "y")
            {
                request = $"SendMessage_{message}:{sendToAllUsers}";
            }
            else
            {
                await Console.Out.WriteLineAsync("\nEnter the targeted user Ids in Comma seperated value format: \n");
                string targetedUserIds = Console.ReadLine();
                request = $"SendMessage_{message}:{sendToAllUsers}-{targetedUserIds}";
            }
            string response = await client.CommunicateWithStreamAsync(request);
        }
        public static async void TakeActionOnDiscardedMenu(SocketClient client)
        {
            Console.WriteLine("\nDiscarded Menu List: \n");
            //GET DISCARDED MENU
            Console.WriteLine("Press 1 for getting detailed feedback\n");
            Console.WriteLine("Press 2 for deleting the items from menu\n");
            string action = Console.ReadLine();
            string request = "DeleteItemsFromDiscardedList";
            if (action == "1")
            {
                await Console.Out.WriteLineAsync("Enter the food id for which you want detailed feedback\n");
                string foodId = Console.ReadLine();
                request = $"GetDetailedfeedbackonfoodItem_{foodId}";
            }
            else
            {
                await Console.Out.WriteLineAsync("Enter Food id of item which you want to remove:\n");
                string foodId = Console.ReadLine();
                request = $"DeleteItemsFromDiscardList_{foodId}";
            }

            string response = await client.CommunicateWithStreamAsync(request);
        }
    }
}
