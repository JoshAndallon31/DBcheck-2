using myUnis.MAUI.Interfaces;
namespace myUnis.MAUI.ViewModels;
using  Xamarin.Essentials;

public class LoginViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly IAlertService _alertService;

    public LoginViewModel() : this(new AlertService())
    {

       

    }

    public LoginViewModel(IAlertService alertService)
    {
        _alertService = alertService;
        LoadCredentials();
        LoginLabelText = "Remember me.";

    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }



    private string _username;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }



  

    private string _loginLabelText;

    public string LoginLabelText
    {
        get { return _loginLabelText; }
        set
        {
            if (_loginLabelText != value)
            {
                _loginLabelText = value;
                OnPropertyChanged(nameof(LoginLabelText));
            }
        }
    }
    
    
    private bool _rememberMe;
    public  bool RememberMe
    {
        get => _rememberMe;
        set
        {
            _rememberMe = value;
            OnPropertyChanged();
            // If the checkbox is checked, save the credentials
            if (value)
            {
                _SaveCredentials();
               LoginLabelText = "Remove credentials.";
            }
            // Otherwise, remove the credentials
            else
            {
                _RemoveCredentials();
                LoginLabelText = "Remember me.";
            }
        }
    }

    private async Task _SaveCredentials()
    {
        try
        {
            // Get the username and password from the input fields
            var username = Username;
            var password = Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await _alertService.ShowAlertAsync("Error", "Username or Password is empty");
                RememberMe = false;
                return;
            }
            // Save them securely using the Secure Storage class
            await SecureStorage.SetAsync("username", username);
            await SecureStorage.SetAsync("password", password);
        }
        catch (Exception ex)
        {
            await _alertService.ShowAlertAsync("Error", ex.Message);
        }
    }

    private async void _RemoveCredentials()
    {
        // Remove the username and password from the Secure Storage
        SecureStorage.Remove("username");
        SecureStorage.Remove("password");
    }
    private async void LoadCredentials()
    {
        // Try to get the username and password from the Secure Storage
        var username = await SecureStorage.GetAsync("username");
        var password = await SecureStorage.GetAsync("password");
        // If they are not null, fill the input fields with them
        if (username != null && password != null)
        {
            Username = username;
            Password = password;
            // Set the checkbox to checked
            RememberMe = true;
        }
    }
}
