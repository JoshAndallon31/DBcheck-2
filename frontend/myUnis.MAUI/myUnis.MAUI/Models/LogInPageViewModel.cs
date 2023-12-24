namespace myUnis.MAUI.Models;

public partial class LogInPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _userName;
    
    [ObservableProperty]
    private string _password;
    
}