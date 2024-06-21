using Domain;
using Domain.DataAccess;
using Domain.Services;
using Domain.Repositories.IRepositories;
using Domain.Repositories;
using Domain.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Services.IServices;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var serviceProvider = host.Services;

        var server = host.Services.GetRequiredService<SocketServer>();
        server.Start();

        Console.WriteLine("Server Started");
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<CafeteriaDbContext>(options =>
                options.UseSqlServer(@"Server=.;Database=CafeteriaDB;Trusted_Connection=True;"));
                services.AddScoped<IFeedbackRepository, FeedbackRepository> ();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IMenuItemRepository, MenuItemRepository>();
                services.AddScoped<NotificationRepository, NotificationRepository>();
                services.AddScoped<IFeedbackService, FeedbackService>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<ILoginService, LoginService>();
                services.AddScoped<IMenuService, MenuService>();
                services.AddScoped<INotificationService, NotificationService>();
                services.AddScoped<IRecommendationEngineService, RecommendationEngineService>();
                services.AddSingleton<SocketServer>();
            });
}
