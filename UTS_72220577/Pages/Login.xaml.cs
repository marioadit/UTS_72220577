using Microsoft.Maui.Controls;
using UTS_72220577.Data;

namespace UTS_72220577.Pages
{
    public partial class Login : ContentPage
    {
        private readonly ccService _service;

        public Login()
        {
            InitializeComponent();
            _service = new ccService(new HttpClient());
        }

        private async void login()
        {
            // After successful login, navigate to home page
            await Shell.Current.GoToAsync("//home");
            Navigation.RemovePage(this); // Removes the Login page from the navigation stack after successful login
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Check if username and password are not empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Username and password cannot be empty.", "OK");
                return;
            }

            // Create a login object based on input
            var loginRequest = new login
            {
                userName = username,
                password = password
            };

            try
            {
                // Call LoginAsync method from ccService
                var token = await _service.LoginAsync(loginRequest);

                if (!string.IsNullOrEmpty(token))
                {
                    // If login is successful, store the token if needed and navigate to home page
                    login();
                }
                else
                {
                    await DisplayAlert("Error", "Login failed, token is missing.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Display an error if login fails
                await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            }
        }

        private async void OnLogOutButtonClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Log Out", "Do you really want to log out?", "Yes", "No");

            if (confirm)
            {
                // Handle logout (you might clear the token and navigate to login)
                // For example, clear token or any session data if applicable
                await Shell.Current.GoToAsync("//login"); // Example navigation to login page after logging out
            }
        }
    }
}
