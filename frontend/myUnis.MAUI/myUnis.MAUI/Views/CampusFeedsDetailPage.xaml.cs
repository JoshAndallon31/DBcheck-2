namespace myUnis.MAUI.Views;

public partial class CampusFeedsDetailPage : ContentPage
{
	public CampusFeedsDetailPage(CampusFeedsDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
