namespace myUnis.MAUI.Views;

public partial class MarketplaceDetailPage : ContentPage
{
	public MarketplaceDetailPage(MarketplaceDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
