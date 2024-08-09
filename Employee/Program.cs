using Data.ModelDTO;
using Data.Models;
using Domain.ModelDTO;
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
        private static int userId;
        public static async Task Main(string[] args)
        {
            var client = new SocketClient("127.0.0.1", 8000);
            bool isLoggedIn = await Login(client);
            if (isLoggedIn)
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
                string request = $"EmployeeLogin_{jsonRequest}";

                string response = await client.CommunicateWithStreamAsync(request);
                Console.WriteLine($"Server response: {response}");

                return response.Contains("Login");
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
                Console.WriteLine("\nEmployee Menu:\n");
                Console.WriteLine("Choose from below options: \n1. View Menu " +
                    "\n2. Get List of meals rolled out by Chef \n" +
                    "3. Vote for next day meals \n4. Provide Feedback on Food Items " +
                    "\n5. View Notifications" +" \n6. View Notifications by user ID" +
                    "\n7. GiveDetailedFeedbackOnDiscardedItems \n8. Create Profile \n9. Logout \n10. Exit");
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
                        await ProvideFeedback(client);
                        break;
                    case "5":
                        await ViewNotifications(client);
                        break;
                    case "6":
                        await ViewNotificationsByUserId(client);
                        break;
                    case "7":
                        await GiveDetailedFeedbackForDiscardedItems(client);
                        break;
                    case "8":
                        CreateProfile(client);
                        break;
                    case "9":
                        Logout(client);
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

        private static async Task ProvideFeedback(SocketClient client)
        {
            try
            {
                Feedback feedback = new Feedback();
                feedback.Date = DateTime.Now;
                Console.WriteLine("\nPlease Provide your feedback");
                feedback.UserId = userId;
                Console.WriteLine("\nEnter which item you had (Item ID): ");
                feedback.MenuItemId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\nEnter feedback message:");
                feedback.Comment = Console.ReadLine();
                Console.WriteLine("\nEnter Rating (1-5): ");
                feedback.Rating = Convert.ToInt32(Console.ReadLine());

                string jsonRequest = JsonConvert.SerializeObject(feedback);
                string request = $"ProvideFeedback_{jsonRequest}";

                var response = await client.CommunicateWithStreamAsync(request);
                Console.WriteLine($"Server response: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async static void VoteForMeals(SocketClient client)
        {
            try 
            {
                Console.WriteLine("\nVote for the items rolled out by Chef\n");
                GetItemsRolledOutByChef(client);
                VotedItems mealVote = new VotedItems();
                mealVote.Date = DateTime.Now;
                mealVote.UserId = userId;
                Console.WriteLine("Enter the food name which you want: ");
                mealVote.FoodName = Console.ReadLine();

                string jsonRequest = JsonConvert.SerializeObject(mealVote);
                string request = $"ItemsVoting_{jsonRequest}";

                var response = await client.CommunicateWithStreamAsync(request);
                Console.WriteLine($"Server response: {response}");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

        }

        private async static void GetItemsRolledOutByChef(SocketClient client)
        {
            try
            {
                string request = $"GetRolledOutItems_{userId}";
                string response = await client.CommunicateWithStreamAsync(request);
                List<ViewMenuDTO> menu = JsonConvert.DeserializeObject<List<ViewMenuDTO>>(response);
                ConvertToTable(menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
        public static async Task ViewMenu(SocketClient client)
        {
            try
            {
                string request = "ViewMenu";
                string response = await client.CommunicateWithStreamAsync(request);
                List<ViewMenuDTO> menu = JsonConvert.DeserializeObject<List<ViewMenuDTO>>(response);
                ConvertToTable(menu);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void Logout(SocketClient client)
        {
            Console.WriteLine("User Logged out, Please Login again to continue");
            Login(client);
        }

        public static async Task ViewNotifications(SocketClient client)
        {
            try
            {
                string request = $"ViewNotifications";
                string response = await client.CommunicateWithStreamAsync(request);
                Console.WriteLine(response);
                List<NotificationDTO> notifications = JsonConvert.DeserializeObject<List<NotificationDTO>>(response);
                ConvertToTable(notifications);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static async Task ViewNotificationsByUserId(SocketClient client)
        {
            try
            {
                string request = $"ViewNotificationsById_{userId}";
                string response = await client.CommunicateWithStreamAsync(request);
                if (response != null)
                {
                    Console.WriteLine(response);
                }
                Console.WriteLine($"No more active notifications for User id: {userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static async Task GiveDetailedFeedbackForDiscardedItems(SocketClient client)
        {
            try
            {
                Console.WriteLine("Provide answer to the questions asked on the discarded items: \n");
                string answers = Console.ReadLine();
                string request = $"GiveDetailedFeedbackOnDiscardedItems_{answers}";
                string response = await client.CommunicateWithStreamAsync(request);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static async void CreateProfile(SocketClient client)
        {
            try
            {
                Profile profile = new Profile();
                Console.WriteLine("Create your profile for food recommendations: \n");
                profile.UserId = userId;
                Console.WriteLine("Please select Veg/Non Veg\n");
                profile.dietType = Console.ReadLine();
                Console.WriteLine("Select Spice level:\n");
                profile.SpiceLevel = Console.ReadLine();
                Console.WriteLine("What do you prefer most (regionwise):\n");
                profile.regionalMealPreference = Console.ReadLine();
                Console.WriteLine("Are you sweeth tooth:t\n");
                profile.isSweetTooth = Console.ReadLine();

                string jsonRequest = JsonConvert.SerializeObject(profile);
                string request = $"CreateProfile_{jsonRequest}";

                var response = await client.CommunicateWithStreamAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

        private static void ConvertToTable(List<NotificationDTO> response)
        {
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("{0,-40} | {1,-25}", "Message", "Date");
            Console.WriteLine(new string('-', 110));

            foreach (var item in response)
            {
                Console.WriteLine("{0,40} | {1,25}", item.Message, item.NotificationDate);
            }
            Console.WriteLine(new string('-', 110));
        }
    }
}
