using System;
using System.Threading.Tasks;
using Domain.ModelDTO;
using Newtonsoft.Json;
using Server;

namespace Admin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Task.Delay(4000);
            using var client = new SocketClient("127.0.0.1", 8000);

            bool isLoggedIn = await Login(client);
            if (isLoggedIn)
            {
                await ShowMenu(client);
            }
            else
            {
                Console.WriteLine("Login failed.");
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
            string request = $"AdminLogin_{jsonRequest}";

            string response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine($"Server response: {response}");

            return response.Contains("Login");
        }

        public static async Task ShowMenu(SocketClient client)
        {
            while (true)
            {
                Console.WriteLine("\nAdmin Menu:\n");
                Console.WriteLine("Choose from below options: \n1. View Menu \n2. Add Item \n3. Update Item \n4. Delete Item \n5. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await ViewMenu(client);
                        break;
                    case "2":
                        await AddItem(client);
                        break;
                    case "3":
                        await UpdateItem(client);
                        break;
                    case "4":
                        await DeleteItem(client);
                        break;
                    case "5":
                        Logout(client);
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Choose again, Invalid choice");
                        break;
                }
            }
        }
        static void Logout(SocketClient client) 
        {
            Console.WriteLine("User Logged out, Please Login again to continue");
            Login(client);
        }

        public static async Task ViewMenu(SocketClient client)
        {
            string request = "ViewMenu";
            string response = await client.CommunicateWithStreamAsync(request);
            var deserializedResponse = JsonConvert.DeserializeObject(response);
            Console.WriteLine($"Menu: {deserializedResponse}");
        }

        public static async Task AddItem(SocketClient client)
        {
            MenuItemDTO item = new MenuItemDTO();
            Console.WriteLine("Enter item name: ");
            item.Name = Console.ReadLine();
            Console.WriteLine("Enter item description: ");
            item.Description = Console.ReadLine();
            Console.WriteLine("Enter item price: ");
            item.Price = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Is the item available? (true/false): ");
            item.IsAvailable = Convert.ToBoolean(Console.ReadLine());
            Console.WriteLine("Enter item meal type: ");
            item.MealType = Console.ReadLine();

            string jsonRequest = JsonConvert.SerializeObject(item);
            string request = $"AddItem_{jsonRequest}";

            var response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine($"Server response: {response}");
        }

        public static async Task UpdateItem(SocketClient client)
        {
            MenuItemDTO item = new MenuItemDTO();
            Console.WriteLine("Enter item Id: ");
            item.Id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter updated item name: ");
            item.Name = Console.ReadLine();
            Console.WriteLine("Enter updated item price: ");
            item.Price = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Update Availability Status: ");
            item.IsAvailable = Convert.ToBoolean(Console.ReadLine());
            Console.WriteLine("Update Meal Type: ");
            item.MealType = Console.ReadLine();

            string jsonRequest = JsonConvert.SerializeObject(item);
            string request = $"UpdateItem_{jsonRequest}";

            var response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine($"Server response: {response}");
        }

        public static async Task DeleteItem(SocketClient client)
        {
            Console.WriteLine("Enter item Id to delete: ");
            int itemId = Convert.ToInt32(Console.ReadLine());

            string request = $"DeleteItem_{itemId}";

            var response = await client.CommunicateWithStreamAsync(request);
            Console.WriteLine($"Server response: {response}");
        }
    }
}

