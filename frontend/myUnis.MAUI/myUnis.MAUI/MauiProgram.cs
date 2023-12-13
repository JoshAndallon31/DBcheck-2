namespace myUnis.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiMaps()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<SampleDataService>();
		builder.Services.AddTransient<CampusFeedsDetailViewModel>();
		builder.Services.AddTransient<CampusFeedsDetailPage>();

		builder.Services.AddSingleton<CampusFeedsViewModel>();

		builder.Services.AddSingleton<CampusFeedsPage>();

		builder.Services.AddTransient<SampleDataService>();
		builder.Services.AddTransient<MarketplaceDetailViewModel>();
		builder.Services.AddTransient<MarketplaceDetailPage>();

		builder.Services.AddSingleton<MarketplaceViewModel>();

		builder.Services.AddSingleton<MarketplacePage>();

		builder.Services.AddSingleton<ChatViewModel>();

		builder.Services.AddSingleton<ChatPage>();

		builder.Services.AddSingleton<NavigatorViewModel>();

		builder.Services.AddSingleton<NavigatorPage>();

		return builder.Build();
	}
}
