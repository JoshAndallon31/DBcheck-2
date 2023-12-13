namespace myUnis.MAUI.Views;

public partial class CampusFeedsPage : ContentPage
{
	CampusFeedsViewModel ViewModel;

	public CampusFeedsPage(CampusFeedsViewModel viewModel)
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
