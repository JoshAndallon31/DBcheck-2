namespace myUnis.MAUI.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class CampusFeedsDetailViewModel : BaseViewModel
{
	[ObservableProperty]
	SampleItem item;
}
