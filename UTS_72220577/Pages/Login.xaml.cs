using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using System.Globalization;

namespace UTS_72220577.Pages;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private String user = "user";

    private async void login()
    {
        await Shell.Current.GoToAsync("//home");
        Navigation.RemovePage(this);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (UsernameEntry.Text == user && PasswordEntry.Text == user)
        {
            login();
        }
        else
        {
            //await DisplayAlert("Error", "Invalid username or password", "OK");
            login();
        }
    }
}