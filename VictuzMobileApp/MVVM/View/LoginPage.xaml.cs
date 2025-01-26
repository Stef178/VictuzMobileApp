using VictuzMobileApp.MVVM.Data;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp.MVVM.View;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim();
        string password = PasswordEntry.Text?.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Fout", "Vul alstublieft zowel e-mailadres als wachtwoord in.", "OK");
            return;
        }

        try
        {
            // Controleer of het de admin-gebruiker is
            if (email == App.AdminUser.Email && password == App.AdminUser.Password)
            {
                App.CurrentUser = App.AdminUser; // Zet admin als actieve gebruiker
                await DisplayAlert("Succes", "Admin succesvol ingelogd!", "OK");

                // **Navigeer naar MainPage in plaats van HomePage**
                Application.Current.MainPage = new NavigationPage(new MainPage());
                return;
            }

            // Haal alle gebruikers op uit de database
            var users = await App.Database.GetAllAsync<Participant>();
            var matchingUser = users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (matchingUser != null)
            {
                await App.Database.SetActiveUser(matchingUser.Id);
                App.CurrentUser = matchingUser;
                await DisplayAlert("Succes", "U bent succesvol ingelogd!", "OK");

                // **Normale gebruiker wordt naar HomePage gestuurd**
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                await DisplayAlert("Fout", "Onjuist e-mailadres of wachtwoord. Probeer het opnieuw.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fout", $"Er is iets misgegaan: {ex.Message}", "OK");
        }
    }

}
