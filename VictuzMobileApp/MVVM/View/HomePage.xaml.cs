namespace VictuzMobileApp.MVVM.View;
using SQLiteBrowser;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new VictuzMobileApp.MVVM.ViewModel.HomePageViewModel();
    }

    private async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        // Bevestig of de gebruiker echt wil uitloggen
        bool confirmLogout = await DisplayAlert("Uitloggen", "Weet u zeker dat u wilt uitloggen?", "Ja", "Nee");
        if (!confirmLogout)
            return;

        try
        {
            // Reset de actieve gebruiker in de database
            await App.Database.SetActiveUser(0); // Zet alle gebruikers op inactief
            App.CurrentUser = null;

            // Navigeer terug naar de StartPage
            await Navigation.PushAsync(new StartPage());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fout", $"Er is iets misgegaan tijdens het uitloggen: {ex.Message}", "OK");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Controleer of de gebruiker een profielafbeelding heeft ingesteld
        if (App.CurrentUser != null && string.IsNullOrEmpty(App.CurrentUser.ProfilePicturePath))
        {
            // Gebruik de standaardafbeelding
            App.CurrentUser.ProfilePicturePath = "person.png";
        }

        // Stel de BindingContext in (indien niet al gedaan)
        BindingContext = App.CurrentUser;
    }


    private async void OnProfileButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnDiscoverButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DiscoverPage());
    }

    private async void OpenDatabaseBrowser(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DatabaseBrowserPage(Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db")));
    }
}