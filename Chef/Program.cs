using Data.ModelDTO;
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
        private static int userId;
        public static async Task Main(string[] args)
        {
            var client = new SocketClient("127.0.0.1", 8000);
            bool isLoggedIn = await Login(client);
            if(isLoggedIn)
            {
                await ShowMenu(client);
            }
            else
            {
                await Login(client);
            }
            Console.ReadLine();
        }

        public static async Task<bool> Login(SocketClient client)
        {
            try
            {
                UserDTO user = new UserDTO();

                Console.WriteLine("Enter your User Id: ");
                user.Id = Convert.ToInt32(Console.ReadLine());
                userId = user.Id;

                Console.WriteLine("Enter your User name: ");
                user.Name = Console.ReadLine();

                string jsonRequest = JsonConvert.SerializeObject(user);
                string request = $"ChefLogin_{jsonRequest}";

                string response = await client.CommunicateWithStreamAsync(request);
                Console.WriteLine($"Server response: {response}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("\nRe-Login:\n");
                await Login(client);
                return false;
            }
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
                    "\n9. Take actions on Discarded items \n10. Add to discard menu " +
                    "\n11. Calculate Average Rating \n12. Calculate Sentiment Scores " +
                    "\n13. Exit");
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
                        ViewMaxVotedItems(client);
                        break;
                    case "8":
                        SendNotifications(client);
                        break;
                    case "9":
                        TakeActionOnDiscardedMenu(client);
                        break;
                    case "10":
                        AddToDiscardMenu(client);
                        break;
                    case "11":
                        CalcAvgRating(client);
                        break;
                    case "12":
                        CalcSentimentScore(client);
                        break;
                    case "13":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Choose again, Invalid choice");
                        break;
                }
            }
        }

        private static async Task ViewAllFeedbacks(SocketClient client)
        {
            string request = "ViewFeedbacks";
            string response = await client.CommunicateWithStreamAsync(request);
            List<FeedbackDTO> feedback = JsonConvert.DeserializeObject<List<FeedbackDTO>>(response);
            ConvertToTable(feedback);
        }

        private static async Task ViewFeedbacksById(SocketClient client)
        {
            Console.WriteLine("Enter the food id for which you want to see the feedback");
            int foodId = Convert.ToInt32(Console.ReadLine());
            string request = $"ViewFeedbacksById_{foodId}";
            string response = await client.CommunicateWithStreamAsync(request);
            List<FeedbackDTO> feedback = JsonConvert.DeserializeObject<List<FeedbackDTO>>(response);
            ConvertToTable(feedback);
        }

        public static async Task ViewMenu(SocketClient client)
        {
            string request = "ViewMenu";
            string response = await client.CommunicateWithStreamAsync(request);
            List<ViewMenuDTO> menu = JsonConvert.DeserializeObject<List<ViewMenuDTO>>(response);
            ConvertToTable(menu);
        }
        public static async Task GetRecommmendedItems(SocketClient client)
        {
            Console.WriteLine("\nEnter the number of items you want to fetch from Recommnendation Engine:");
            var noOfRecommendedItems = Console.ReadLine();
            string request = $"GetRecommendedMeals_{noOfRecommendedItems}";
            string response = await client.CommunicateWithStreamAsync(request);
            List<ViewMenuDTO> recommendedMenu = JsonConvert.DeserializeObject<List<ViewMenuDTO>>(response);
            Console.WriteLine($"Top Recommmended Meals:");
            ConvertToTable(recommendedMenu);

        }
        public static async void RollOutItems(SocketClient client)
        {
            try
            {
                Console.WriteLine("\nEnter the Item Id(s) which you want to roll out for next day:");
                string itemIds = Console.ReadLine();
                string[] itemIdsArray = itemIds.Split(',');
                List<int> itemIdsInt = itemIdsArray.Select(int.Parse).ToList();

                string request = $"RollOutItems_{itemIds}";
                string response = await client.CommunicateWithStreamAsync(request);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
        static void Logout(SocketClient client)
        {
            Console.WriteLine("User Logged out, Please Login again to continue");
            Login(client);
        }
        public static async void SendNotifications(SocketClient client)
        {
            try
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
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
        public static async void TakeActionOnDiscardedMenu(SocketClient client)
        {
            try
            {
                Console.WriteLine("\nDiscarded Menu List: \n");
                var discardedMenuJson = GetDiscardedMenuAsync(client);
                Console.WriteLine($"{discardedMenuJson}");
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
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        private static async void AddToDiscardMenu(SocketClient client)
        {
            string request = $"ReviewMenuItems";
            string response = await client.CommunicateWithStreamAsync(request); 
        }
        private static async void ViewMaxVotedItems(SocketClient client)
        {
            var currentDate = DateTime.Now.Date;
            string request = $"ViewMaxVotedItems_{currentDate}";
            string response = await client.CommunicateWithStreamAsync(request);
            await Console.Out.WriteLineAsync(response);
        }
        private static async void CalcAvgRating(SocketClient client)
        {
            string request = $"CalcAvgRating";
            string response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine(response);
        }
        private static async void CalcSentimentScore(SocketClient client)
        {
            string request = $"CalcSentimentScore";
            string response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine(response);
        }

        private static async Task GetDiscardedMenuAsync(SocketClient client)
        {
            string request = "$GetDiscardedMenu";
            string response = await client.CommunicateWithStreamAsync(request);
            List<ViewMenuDTO> discardedMenu = JsonConvert.DeserializeObject<List<ViewMenuDTO>>(response);
            ConvertToTable(discardedMenu);
        }

        private static void ConvertToTable(List<FeedbackDTO> response)
        {
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("{0,-10} | {1,-10} | {2,-40} | {3,-6} | {4,-25}", "Menu ID", "User ID", "Comment", "Rating", "Date");
            Console.WriteLine(new string('-', 100));

            foreach (var item in response)
            {
                Console.WriteLine("{0,-10} | {1,-10} | {2,-40} | {3,-6} | {4,-25}", item.MenuItemId, item.UserId, item.Comment, item.Rating, item.Date);
            }
            Console.WriteLine(new string('-', 110));
        }
        private static void ConvertToTable(List<ViewMenuDTO> response)
        {
            Console.WriteLine(new string('-', 110));
            Console.WriteLine("{0,-3} | {1,-25} | {2,-50} | {3,-8} | {4,-8}", "Id", "Name", "Description", "Price", "MealType");
            Console.WriteLine(new string('-', 110));

            foreach (var item in response)
            {
                Console.WriteLine("{0,-3} | {1,-25} | {2,-50} | {3,-8} | {4,-8}", item.Id, item.Name, item.Description, item.Price, item.MealType);
            }
            Console.WriteLine(new string('-', 110));
        }
    }
}
