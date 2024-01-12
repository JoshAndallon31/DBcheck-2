using myUnis.MAUI.Interfaces;

namespace myUnis.MAUI.Views;
using System;
using System.Collections.Generic;
public partial class LogInPage :   INotifyPropertyChanged
{
    private readonly ILogInRepository _logInRepository = new LogInService();
    private readonly IDispatcherTimer _timer;
    private int _counter;
    private int _value;
    private DateTime _startTime;
    private string _countdownText { get; set; } 
    public string CountdownText
    {
        get { return _countdownText; }
        set
        {
            if (_countdownText != value)
            {
                _countdownText = value;
                OnPropertyChanged(nameof(CountdownText));
            }
        }
    }
    public LogInPage()
    {
        InitializeComponent();
        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _counter = 0;
        var CountdownLabel = new Label
        {
            FontSize = 15,
            TextColor = Colors.Red,
            FontFamily = "PoppinsRegular",
            
        };
        CountdownLabel.BindingContext = this;
        CountdownLabel.SetBinding(Label.TextProperty, "CountdownText");
    }
    private void OnEyeButtonClicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        eyeButton.Source = PasswordEntry.IsPassword ? "hidepass.png" : "showpass.png";
    }

    readonly Dictionary<string, int> _lockoutCounts = new Dictionary<string, int>();
    const double _LockoutDurationInMinutes = 0.5;
    
    private async void LoginButton(object sender, EventArgs e)
    {
       // UserInfo userInfo = await _logInRepository.Login(UsernameEntry.Text, PasswordEntry.Text);
        if (string.IsNullOrEmpty(UsernameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Please enter both username and password.", "OK");
        }
        // replace else if condition with (userinfo != null)
        else if (UsernameEntry.Text == "admin" && PasswordEntry.Text == "admin")
        {
            await DisplayAlert("Success", "Login Successful!", "OK");
            Application.Current.MainPage = new NavigationPage(new Biometrics());
        }
        else
        {
            _counter++;
            if (_counter == 4)
            {
                CountdownLabel.IsVisible = true;
                Loginbutton.IsEnabled = false;
                UsernameEntry.IsEnabled = false;
                PasswordEntry.IsEnabled = false;
                if(_lockoutCounts.TryGetValue(UsernameEntry.Text,out _value))
                {
                    _lockoutCounts[UsernameEntry.Text] = ++_value;
                    if (_value == 3)
                    {
                        Application.Current.MainPage = new NavigationPage(new ForgetPasswordPage());
                        _lockoutCounts[UsernameEntry.Text] = 0;
                        return;
                    }
                }
                else
                {
                    _lockoutCounts.TryAdd(UsernameEntry.Text, 1);
                }
                _timer.Start();
                _startTime = DateTime.Now;
                
            }
            else
            {
                await DisplayAlert("Error", "Invalid Username or Password", "OK");
            }
        }
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var lockoutCount = _lockoutCounts[username];
        var remainingTime = TimeSpan.FromMinutes(_LockoutDurationInMinutes * lockoutCount) - (DateTime.Now - _startTime);
        if (remainingTime <= TimeSpan.Zero)
        {
            _timer.Stop();
            _counter = 0;
            Loginbutton.IsEnabled = true;
            UsernameEntry.IsEnabled = true;
            PasswordEntry.IsEnabled = true;
            CountdownLabel.IsVisible = false;
           
        }
        else
        {
            CountdownText = $"Please wait {remainingTime:mm\\:ss} before trying again.";
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private  void OnForgotPasswordLabelTapped(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new ForgetPasswordPage());
    }
    private  void OnSignUpLabelTapped(object sender, EventArgs e)
    {
       //Put here the navigation of sign up page
    }
}