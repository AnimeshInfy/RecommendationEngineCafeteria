using Domain.Models;
using Domain.Services.IServices;
using Server.RequestHandler;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketServer
{
    private TcpListener _listener;
    private LoginRequestHandler _loginRequestHandler;
    private MenuRequestHandler _menuHandler;
    private IUserService _userService;
    private IMenuService _menuService;
    public SocketServer(IUserService userService)
    {
        _userService = userService;
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
        var bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
        var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        var response = ProcessRequest(request);

        var responseData = Encoding.UTF8.GetBytes(await response);
        await networkStream.WriteAsync(responseData, 0, responseData.Length);
    }

    private async Task<string> ProcessRequest(string request)
    {
        if (request.Contains("Login"))
        {
            return await _loginRequestHandler.HandleRequestAsync(request);
        }

        if (request.StartsWith("ViewMenu"))
        {
            return await _menuHandler.HandleRequestAsync(request);
        }
        if (request.StartsWith(""))
        {
            return ExampleHandler(request);
        }
        return "Unknown request";
    }

    private string ExampleHandler(string request)
    {
        var message = request.Substring(5);
        return $"Example: {message}";
    }
}

