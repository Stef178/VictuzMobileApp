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
		string username = UsernameEntry.Text?.Trim();
		string password = PasswordEntry.Text?.Trim();

		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			await DisplayAlert("Fout", "Vul alstublieft zowel gebruikersnaam als wachtwoord in.", "OK");
			return;
		}

		try
		{
			var user = await App.Database.GetAllAsync<Participant>();
			var matchingUser = user.FirstOrDefault(u => u.Name == username && u.Password == password);

            if (matchingUser != null)
            {
                App.CurrentUser = matchingUser;
                await DisplayAlert("Succes", "U bent succesvol ingelogd!", "OK");
                await Navigation.PushAsync(new HomePage());
            }

            else
            {
				await DisplayAlert("Fout", "Onjuiste gebruikersnaam of wachtwoord. Probeer het opnieuw.", "OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Fout", $"Er is iets misgegaan: {ex.Message}", "OK");
		}
	}
}
