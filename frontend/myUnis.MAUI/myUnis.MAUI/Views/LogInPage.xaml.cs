using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myUnis.MAUI.Views;

public partial class LogInPage : ContentPage
{
    public LogInPage()
    {
        InitializeComponent();
    }
    private void OnEyeButtonClicked(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        eyeButton.Source = passwordEntry.IsPassword ? "hidepass.png" : "showpass.png";
    }

    public void OnClickedFingerprint(object sender, EventArgs e)

    {
    }

}