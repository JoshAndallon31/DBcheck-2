using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.Fingerprint;

namespace myUnis.MAUI.Views
{
    public partial class Biometrics : ContentPage
    {
        private bool userChoice;

        public Biometrics()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            if (userChoice)
            {
                var hasFingerprint = await CrossFingerprint.Current.IsAvailableAsync(true);

                if (hasFingerprint)
                {
                    await DisplayAlert("Success", "Biometric authentication is available on this device.", "Ok");
                    Application.Current.MainPage = new BiometricConfirmation
                    {
                        BackgroundColor = Colors.White
                    };
                }
                else
                {
                    await DisplayAlert("Error", "Biometric authentication is not available on this device.", "Ok");
                    Application.Current.MainPage = new AppShell();
                }
            }
            else
            {
                Application.Current.MainPage = new AppShell();
            }
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            userChoice = e.Value;
        }
        private async  void returnButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}