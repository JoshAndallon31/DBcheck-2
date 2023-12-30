using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace myUnis.MAUI;
using System.Timers;
using Xamarin.Essentials;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

public partial class App : Application
{
	private readonly Timer _timer = new Timer();
	
	public App()
	{
		InitializeComponent();

		
		var services = new ServiceCollection();
		

		// Build the service provider
		var serviceProvider = services.BuildServiceProvider();

		// Set the service provider for the application
		ServiceProvider = serviceProvider;

		MainPage = new LogInPage();
		_timer = new Timer(30000); // Check for internet connection every minute
		_timer.Elapsed += OnTimerElapsed;
		_timer.Start();
		var hasConnection = Connectivity.NetworkAccess == NetworkAccess.Internet;
		 async void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			var hasConnection = Connectivity.NetworkAccess == NetworkAccess.Internet;

			if (hasConnection)
			{
				_timer.Stop();
				await MainPage.Navigation.PushAsync(new LogInPage());
			}
			else
			{
				Device.BeginInvokeOnMainThread(async () =>
				{
					await MainPage.DisplayAlert("Error", "Please check your internet connection and try again.", "Ok");
					
				});
			}
		}

		if (!hasConnection)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await MainPage.DisplayAlert("Error", "Please check your internet connection and try again.", "Ok");
				Application.Current.Quit();
			});
		}
		
		







	}
	protected override async void OnStart()
	{
		// Check if the user has enabled fingerprint authentication
		var fingerprintEnabled = await SecureStorage.GetAsync("fingerprintEnabled");

		if (fingerprintEnabled == "true")
		{
			// Create an authentication request configuration
			var request = new AuthenticationRequestConfiguration("Login with fingerprint", "Please scan your fingerprint to login");

			// Call the CrossFingerprint plugin to authenticate the user
			var result = await CrossFingerprint.Current.AuthenticateAsync(request);

			// Check the result of the authentication
			if (result.Authenticated)
			{
				// Navigate to the home page
				await MainPage.Navigation.PushAsync(new AppShell());
			}
			else
			{
				// Display an error message
				await MainPage.DisplayAlert("Error", result.ErrorMessage, "OK");
			}
		}
	}

	public IServiceProvider ServiceProvider { get; private set; }
	}

