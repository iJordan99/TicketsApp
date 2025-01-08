using Microsoft.Extensions.Logging;
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
			.RegisterViewsAndViewModels();

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
		mauiAppBuilder.Services.AddSingleton<RestApiService>();
		mauiAppBuilder.Services.AddSingleton<LoginPageViewModel>();
		return mauiAppBuilder;
	}
}
