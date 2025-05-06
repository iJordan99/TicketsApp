using System.Text.Json;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using TicketsApp.Interfaces;
using TicketsApp.Parsers;
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
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Lexend-Black.ttf", "LexendBlack");
                fonts.AddFont("Lexend-Bold.ttf", "LexendBold");
                fonts.AddFont("Lexend-ExtraBold.ttf", "LexendExtraBold");
                fonts.AddFont("Lexend-ExtraLight.ttf", "LexendExtraLight");
                fonts.AddFont("Lexend-Light.ttf", "LexendLight");
                fonts.AddFont("Lexend-Medium.ttf", "LexendMedium");
                fonts.AddFont("Lexend-Regular.ttf", "LexendRegular");
                fonts.AddFont("Lexend-SemiBold.ttf", "LexendSemiBold");
                fonts.AddFont("Lexend-Thin.ttf", "LexendThin");
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
        mauiAppBuilder.Services.AddTransient<TicketDetailsPage>();
        //ViewModel
        mauiAppBuilder.Services.AddTransient<HomePageViewModel>();
        mauiAppBuilder.Services.AddTransient<LoginPageViewModel>();
        mauiAppBuilder.Services.AddTransient<TicketDetailsViewModel>();
        return mauiAppBuilder;
    }

    private static void RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IAppState, AppState>();

        mauiAppBuilder.Services.AddSingleton<IAuthService, AuthService>();


        mauiAppBuilder.Services.AddSingleton<IEngineerTicketService, EngineerTicketService>();

        mauiAppBuilder.Services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        });

        mauiAppBuilder.Services.AddSingleton<HttpClient>(_ =>
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        });

        mauiAppBuilder.Services.AddSingleton(typeof(TicketParsingConfig));
        mauiAppBuilder.Services.AddSingleton(typeof(GlobalParsingConfig));


        mauiAppBuilder.Services.AddSingleton<IJsonParsingHelper, JsonParsingHelper>();

        mauiAppBuilder.Services.AddSingleton<ITicketParser, TicketParser>();
        mauiAppBuilder.Services.AddSingleton<ITicketService, TicketService>();
        mauiAppBuilder.Services.AddSingleton<IErrorParser, ErrorParser>();
        mauiAppBuilder.Services.AddSingleton<IPostApiResponseService, PostApiResponseService>();

        mauiAppBuilder.Services.AddSingleton<IJsonParsingHelper, JsonParsingHelper>();
        mauiAppBuilder.Services.AddSingleton<IUserParser, UserParser>();
        mauiAppBuilder.Services.AddSingleton<ICommentParser, CommentParser>(sp =>
            new CommentParser(sp.GetRequiredService<IUserParser>(), sp.GetRequiredService<IJsonParsingHelper>()));
    }
}