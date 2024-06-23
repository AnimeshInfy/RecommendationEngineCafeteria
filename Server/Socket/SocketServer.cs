using Domain.Models;
using Domain.Services.IServices;
using Server.RequestHandler;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class SocketServer
{
    private TcpListener _listener;
    private LoginRequestHandler _loginRequestHandler;
    private MenuRequestHandler _menuHandler;
    private IUserService _userService;
    private IMenuService _menuService;

    public SocketServer(IUserService userService, IMenuService menuService)
    {
        _userService = userService;
        _menuService = menuService;
        _loginRequestHandler = new LoginRequestHandler(_userService);
        _menuHandler = new MenuRequestHandler(_menuService);
    }

    public void Start()
    {
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8000);
        _listener.Start();
        Console.WriteLine("Socket server started on port 8000.");

        while (true)
        {
            try
            {
                var client = _listener.AcceptTcpClient();
                Task.Run(() => ClientHandler(client));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    private async Task ClientHandler(TcpClient client)
    {
        const int BufferSize = 4096;

        using var networkStream = client.GetStream();
        var buffer = new byte[BufferSize];

        while (client.Connected)
        {
            try
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var response = await ProcessRequestAsync(request);

                var responseData = Encoding.UTF8.GetBytes(response);
                await networkStream.WriteAsync(responseData, 0, responseData.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                break;
            }
        }
        client.Close();
    }

    private async Task<string> ProcessRequestAsync(string request)
    {
        if (request.Contains("Login"))
        {
            return await _loginRequestHandler.HandleRequestAsync(request);
        }

        if (request.StartsWith("ViewMenu") || request.StartsWith("AddItem") || request.StartsWith("UpdateItem") || request.StartsWith("DeleteItem"))
        {
            return await _menuHandler.HandleRequestAsync(request);
        }

        return "Unknown request";
    }
}
