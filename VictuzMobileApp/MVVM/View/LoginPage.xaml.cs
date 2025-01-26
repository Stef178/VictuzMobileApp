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
        // Haal de ingevoerde e-mail en wachtwoord op
        string email = EmailEntry.Text?.Trim();
        string password = PasswordEntry.Text?.Trim();

        // Controleer of beide velden zijn ingevuld
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Fout", "Vul alstublieft zowel e-mailadres als wachtwoord in.", "OK");
            return;
        }

        try
        {
            // Haal alle gebruikers op uit de database
            var users = await App.Database.GetAllAsync<Participant>();

            // Zoek de gebruiker met de opgegeven e-mailadres en wachtwoord
            var matchingUser = users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (matchingUser != null)
            {
                // Markeer de gebruiker als actief in de database
                await App.Database.SetActiveUser(matchingUser.Id);

                // Sla de actieve gebruiker op in de statische App-property
                App.CurrentUser = matchingUser;

                // Geef een succesmelding en navigeer naar de HomePage
                await DisplayAlert("Succes", "U bent succesvol ingelogd!", "OK");
                await Navigation.PushAsync(new HomePage());
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
