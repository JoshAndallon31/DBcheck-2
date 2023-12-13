namespace myUnis.MAUI.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class MarketplaceDetailViewModel : BaseViewModel
{
	[ObservableProperty]
	SampleItem item;
}
