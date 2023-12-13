namespace myUnis.MAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(MarketplaceDetailPage), typeof(MarketplaceDetailPage));
		Routing.RegisterRoute(nameof(CampusFeedsDetailPage), typeof(CampusFeedsDetailPage));
	}
}
