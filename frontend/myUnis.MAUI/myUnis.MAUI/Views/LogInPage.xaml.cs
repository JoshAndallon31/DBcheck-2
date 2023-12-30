namespace myUnis.MAUI.Views;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Essentials;



public partial class LogInPage : ContentPage , INotifyPropertyChanged
{
    IDispatcherTimer timer;
    private int counter;
    private int value;
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
       
        timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        counter = 0;
        CountdownLabel.BindingContext = this;
        CountdownLabel.FontSize = 15;
        CountdownLabel.TextColor = Colors.Red;
        CountdownLabel.FontFamily = "PoppinsRegular";
        CountdownLabel.SetBinding(Label.TextProperty, "CountdownText");
        
    }
    private void OnEyeButtonClicked(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        eyeButton.Source = passwordEntry.IsPassword ? "hidepass.png" : "showpass.png";
    }
    

    

    readonly Dictionary<string, int> _lockoutCounts = new Dictionary<string, int>();
    const int _lockoutDuration = 1;
    private async void LoginButton(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(UsernameEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
        {
            await DisplayAlert("Error", "Please enter both username and password.", "OK");
        }
        else if (UsernameEntry.Text.Equals("admin") && passwordEntry.Text.Equals("admin"))
        {
           
            await DisplayAlert("Success", "Login Successful!", "OK");
            Application.Current.MainPage = new NavigationPage(new Biometrics());
        }
        else
        {
            counter++;
            if (counter == 4)
            {
                CountdownLabel.IsVisible = true;
                Loginbutton.IsEnabled = false;
                UsernameEntry.IsEnabled = false;
                passwordEntry.IsEnabled = false;
                if(_lockoutCounts.TryGetValue(UsernameEntry.Text,out value))
                {
                    _lockoutCounts[UsernameEntry.Text] = ++value;
                    if (value == 3)
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
                timer.Start();
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
        var remainingTime = TimeSpan.FromMinutes(_lockoutDuration * lockoutCount) - (DateTime.Now - _startTime);
        if (remainingTime <= TimeSpan.Zero)
        {
            timer.Stop();
            counter = 0;
            Loginbutton.IsEnabled = true;
            UsernameEntry.IsEnabled = true;
            passwordEntry.IsEnabled = true;
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
    
    private async void OnForgotPasswordLabelTapped(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new ForgetPasswordPage());
    }
   
    
}