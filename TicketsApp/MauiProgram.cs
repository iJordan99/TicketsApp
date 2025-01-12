using System.Text.Json;
using Microsoft.Extensions.Logging;
using TicketsApp.Interfaces;
using TicketsApp.Services;
using TicketsApp.ViewModels;
using TicketsApp.Views;

namespace TicketsApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterViewsAndViewModels()
            .RegisterServices();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterViewsAndViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        //Pages
        mauiAppBuilder.Services.AddTransient<HomePage>();
        mauiAppBuilder.Services.AddTransient<LoginPage>();
        //ViewModel
        mauiAppBuilder.Services.AddTransient<HomePageViewModel>();
        mauiAppBuilder.Services.AddTransient<LoginPageViewModel>();
        return mauiAppBuilder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IAppState, AppState>();
        
        mauiAppBuilder.Services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        
        mauiAppBuilder.Services.AddSingleton<HttpClient>(sp =>
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        });
        
        mauiAppBuilder.Services.AddSingleton<IAuthService, AuthService>();
        return mauiAppBuilder;
    }
}
