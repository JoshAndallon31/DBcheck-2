namespace myUnis.MAUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		
		var services = new ServiceCollection();
		

		// Build the service provider
		var serviceProvider = services.BuildServiceProvider();

		// Set the service provider for the application
		ServiceProvider = serviceProvider;
		MainPage = new LogInPage();
		
	}

	public IServiceProvider ServiceProvider { get; private set; }
	}

