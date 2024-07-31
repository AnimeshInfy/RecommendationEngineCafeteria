using Data.ModelDTO;
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
    private FeedbackRequestHandler _feedbackHandler;
    private IUserService _userService;
    private IMenuService _menuService;
    private INotificationService _notificationService;
    private IFeedbackService _feedbackService;
    private IRecommendationEngineService _recommendationEngineService;
    private RecommendationHandler _recommendationHandler;
    private NotificationHandler _notificationHandler;
    private IRatingServce _ratingServce;
    private ISentimentsAnalysisService _sentimentsAnalysisService;

    public SocketServer(IUserService userService, IMenuService menuService,
        IFeedbackService feedbackService, IRecommendationEngineService recommendationEngineService,
        INotificationService notificationService, IRatingServce ratingServce,
        ISentimentsAnalysisService sentimentsAnalysisService)
    {
        _userService = userService;
        _menuService = menuService;
        _feedbackService = feedbackService;
        _notificationService = notificationService;
        _ratingServce = ratingServce;
        _sentimentsAnalysisService = sentimentsAnalysisService;
        _recommendationEngineService = recommendationEngineService;
        _loginRequestHandler = new LoginRequestHandler(_userService);
        _menuHandler = new MenuRequestHandler(_menuService, _ratingServce, _sentimentsAnalysisService);
        _recommendationHandler = new RecommendationHandler(_recommendationEngineService, _menuService);
        _feedbackHandler = new FeedbackRequestHandler(_feedbackService, _notificationService, _menuService);
        _notificationHandler = new NotificationHandler(_notificationService);
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
            return await HandleLoginRequestAsync(request);
        }

        if (request.StartsWith("ViewMenu") || request.StartsWith("AddItem")
            || request.StartsWith("UpdateItem") || request.StartsWith("DeleteItem"))
        {
            return await HandleMenuRequestAsync(request);
        }

        if (request.Contains("ProvideFeedback") || request.Contains("ViewFeedbacks")
            || request.Contains("ViewFeedbacksById"))
        {
            return await HandleFeedbackRequestAsync(request);
        }

        if (request.Contains("GetRecommendedMeals"))
        {
            return await HandleGetRecommendedMealsAsync(request);
        }

        if (request.Contains("RollOutItems"))
        {
            return await HandleRollOutItemsAsync(request);
        }

        if (request.Contains("GetRolledOutItems"))
        {
            return await HandleGetRolledOutItemsAsync(request);
        }

        if (request.Contains("ItemsVoting"))
        {
            return await HandleItemsVotingAsync(request);
        }

        if (request.Contains("SendMessage"))
        {
            return await HandleSendMessageAsync(request);
        }

        if (request == "ViewNotifications")
        {
            return await HandleViewNotificationsAsync(request);
        }

        if (request.Contains("ViewNotificationsById"))
        {
            return await HandleViewNotificationsByIdAsync(request);
        }

        if (request.Contains("DeleteItemsFromDiscardList"))
        {
            return await HandleDeleteItemsFromDiscardListAsync(request);
        }

        if (request.Contains("GetDetailedfeedbackonfoodItem"))
        {
            return await HandleGetDetailedFeedbackOnFoodItemAsync(request);
        }

        if (request.Contains("ReviewMenuItems"))
        {
            return await HandleReviewMenuItemsAsync(request);
        }

        if (request.Contains("GiveDetailedFeedbackOnDiscardedItems_"))
        {
            return await HandleGiveDetailedFeedbackOnDiscardedItemsAsync(request);
        }

        if (request.Contains("CreateProfile"))
        {
            return await HandleCreateProfileAsync(request);
        }
        if (request.Contains("ViewMaxVotedItems"))
        {
            return await HandleViewMaxVotedItemsAsync(request);
        }
        if (request.Contains("CalcAvgRating") || request.Contains("CalcSentimentScore"))
        {
            return await HandleCalcAvgScoresAsync(request);
        }
        if (request.Contains("$GetDiscardedMenu"))
        {
            return await HandleGetDiscardMenuAsync(request);
        }
        return "Unknown request";
    }

    private async Task<string> HandleViewMaxVotedItemsAsync(string request)
    {
        return await _menuHandler.ViewMaxVotedItems(request);
    }

    private async Task<string> HandleLoginRequestAsync(string request)
    {
        return await _loginRequestHandler.HandleRequestAsync(request);
    }

    private async Task<string> HandleMenuRequestAsync(string request)
    {
        return await _menuHandler.HandleRequestAsync(request);
    }

    private async Task<string> HandleFeedbackRequestAsync(string request)
    {
        return await _feedbackHandler.HandleRequestAsync(request);
    }

    private async Task<string> HandleGetRecommendedMealsAsync(string request)
    {
        return await _recommendationHandler.GetRecommendedMeals(request);
    }

    private async Task<string> HandleRollOutItemsAsync(string request)
    {
        return await _recommendationHandler.RollOutItems(request);
    }

    private async Task<string> HandleGetRolledOutItemsAsync(string request)
    {
        return await _recommendationHandler.GetRolledOutItems(request);
    }

    private async Task<string> HandleItemsVotingAsync(string request)
    {
        return await _recommendationHandler.ItemsVoting(request);
    }

    private async Task<string> HandleSendMessageAsync(string request)
    {
        await _notificationHandler.SendNotifications(request);
        return "Notification sent";
    }

    private async Task<string> HandleViewNotificationsAsync(string request)
    {
        return await _notificationHandler.ViewNotifications(request);
    }

    private async Task<string> HandleViewNotificationsByIdAsync(string request)
    {
        return await _notificationHandler.ViewNotificationsById(request);
    }

    private async Task<string> HandleDeleteItemsFromDiscardListAsync(string request)
    {
        return await _menuHandler.HandleRequestAsync(request);
    }

    private async Task<string> HandleGetDetailedFeedbackOnFoodItemAsync(string request)
    {
        return await _feedbackHandler.HandleRequestAsync(request);
    }

    private async Task<string> HandleReviewMenuItemsAsync(string request)
    {
        return await _menuHandler.HandleRequestAsync(request);
    }

    private async Task<string> HandleGiveDetailedFeedbackOnDiscardedItemsAsync(string request)
    {
        return await _feedbackHandler.HandleRequestAsync(request);
    }

    private async Task<string> HandleCreateProfileAsync(string request)
    {
        return await _recommendationHandler.CreateProfile(request);
    }
    private async Task<string> HandleCalcAvgScoresAsync(string request)
    {
        if (request == "CalcAvgRating")
        {
            return await _menuHandler.CalcAvgRatingAsync(request);
        }
        else if (request == "CalcSentimentScore")
        {
            return await _menuHandler.CalcSentimentScoreAsync(request);
        }
        return "Unknown Request";
    }
    private async Task<string> HandleGetDiscardMenuAsync(string request)
    {
        return await _menuHandler.GetDiscardedMenu(request);
    }
}
