namespace myUnis.MAUI.Views;

public partial class MarketplacePage : ContentPage
{
	MarketplaceViewModel ViewModel;

	public MarketplacePage(MarketplaceViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = ViewModel = viewModel;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		await ViewModel.LoadDataAsync();
	}
}
