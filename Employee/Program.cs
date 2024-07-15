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
        public static async Task Main(string[] args)
        {
            var client = new SocketClient("127.0.0.1", 8000);
            bool isLoggedIn = await Login(client);
            if (isLoggedIn)
            {
                ShowMenu(client);
            }
            else { 
                await Login(client); 
            }
            Console.ReadLine();
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
                    "\n7. GiveDetailedFeedbackOnDiscardedItems \n8. Logout \n9. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewMenu(client);
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
                        ViewNotifications(client);
                        break;
                    case "6":
                        ViewNotificationsByUserId(client);
                        break;
                    case "7":
                        GiveDetailedFeedbackForDiscardedItems(client);
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
            Console.WriteLine("\nVote for the items rolled out by Chef\n");
            GetItemsRolledOutByChef(client);
            VotedItems mealVote = new VotedItems();
            mealVote.Date = DateTime.Now;
            Console.WriteLine("\nPlease Provide your user Id");
            mealVote.UserId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter meal type for which you want to vote");
            await Console.Out.WriteLineAsync("meal type options");
            int mealType = Convert.ToInt32(Console.ReadLine());
            if (mealType == 1)
            {
                mealVote.MealTypes = "Breakfast";
            }
            else if (mealType == 2)
            {
                mealVote.MealTypes = "Lunch";
            }
            else if (mealType == 3)
            {
                mealVote.MealTypes = "Dinner";
            }
            else
            {
                Console.WriteLine("Invalid choice of meal types");
            }
            Console.WriteLine("Enter the food name which you want: ");
            mealVote.FoodName = Console.ReadLine();

            string jsonRequest = JsonConvert.SerializeObject(mealVote);
            string request = $"ItemsVoting_{jsonRequest}";

            var response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine($"Server response: {response}");

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

        public static async Task ViewNotifications(SocketClient client)
        {
            string request = $"ViewNotifications";
            string response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine(response);
        }
        public static async Task ViewNotificationsByUserId(SocketClient client)
        {
            Console.WriteLine("Enter your User Id: \n");
            int userId = Convert.ToInt32(Console.ReadLine());
            string request = $"ViewNotificationsById_{userId}";
            string response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine(response);
        }
        public static async Task GiveDetailedFeedbackForDiscardedItems(SocketClient client)
        {
            Console.WriteLine("Provide answer to the questions asked on the discarded items: \n");
            string answers = Console.ReadLine();
            string request = $"GiveDetailedFeedbackOnDiscardedItems_{answers}";
            string response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine(response);
        }
        private static async  void CreateProfile(SocketClient client)
        {
            Profile profile = new Profile();    
            Console.WriteLine("Create your profile for food recommendations: \n");
            Console.WriteLine("Enter user id:\n");
            profile.UserId = Convert.ToInt32(Console.ReadLine());   
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
    }
}
