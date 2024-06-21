using System;
using System.Text.Json.Serialization;
using Domain;
using Domain.ModelDTO;
using Newtonsoft.Json;
using Server;

namespace Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new SocketClient("127.0.0.1", 8000);
            Login(client);
            ShowMenu(client);
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
            string request = $"AdminLogin_{jsonRequest}";

            client.SendMessage(request);

            string response = client.ReceiveMessage();
            Console.WriteLine($"Server response: {response}");

            loginStatus = response.Contains("Login");

            return loginStatus;
        }

    public static void ShowMenu(SocketClient client)
        {
            while (true)
            {
                Console.WriteLine("Admin Menu:\n");
                Console.WriteLine("Choose from below options: \n1. View Menu \n2. Add Item \n3. Update Item \n4. Delete Item \n5. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewMenu(client);
                        break;
                    case "2":
                        AddItem(client);
                        break;
                    case "3":
                        UpdateItem(client);
                        break;
                    case "4":
                        DeleteItem(client);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Choose again, Invalid choice");
                        break;
                }
            }
        }

        public static void ViewMenu(SocketClient client)
        {
            client.SendMessage("ViewMenu");
            string response = client.ReceiveMessage();
            Console.WriteLine($"Menu: {response}");
        }

        public static void AddItem(SocketClient client)
        {
            MenuItemDTO item = new MenuItemDTO();
            Console.WriteLine("Enter item Id: ");
            item.Id = Convert.ToInt32(Console.ReadLine());
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

            client.SendMessage(request);
            string response = client.ReceiveMessage();
            Console.WriteLine($"Server response: {response}");
        }

        public static void UpdateItem(SocketClient client)
        {
            MenuItemDTO item = new MenuItemDTO();
            Console.WriteLine("Enter item Id: ");
            item.Id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter updated item name: ");
            item.Name = Console.ReadLine();
            Console.WriteLine("Enter updated item price: ");
            item.Price = Convert.ToDecimal(Console.ReadLine());

            string jsonRequest = JsonConvert.SerializeObject(item);
            string request = $"UpdateItem_{jsonRequest}";

            client.SendMessage(request);
            string response = client.ReceiveMessage();
            Console.WriteLine($"Server response: {response}");
        }

        public static void DeleteItem(SocketClient client)
        {
            Console.WriteLine("Enter item Id to delete: ");
            int itemId = Convert.ToInt32(Console.ReadLine());

            string request = $"DeleteItem_{itemId}";

            client.SendMessage(request);
            string response = client.ReceiveMessage();
            Console.WriteLine($"Server response: {response}");
        }
    }
}



