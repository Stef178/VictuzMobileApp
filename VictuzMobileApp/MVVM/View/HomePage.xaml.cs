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
        bool confirmLogout = await DisplayAlert("Uitloggen", "Weet u zeker dat u wilt uitloggen?", "Ja", "Nee");
        if (!confirmLogout)
            return;

        try
        {
            await App.Database.SetActiveUser(0);
            App.CurrentUser = null;

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

        if (App.CurrentUser != null && string.IsNullOrEmpty(App.CurrentUser.ProfilePicturePath))
        {
            App.CurrentUser.ProfilePicturePath = "person.png";
        }

        BindingContext = App.CurrentUser;
    }


    private async void OnProfileButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnDiscoverButtonClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new VictuzMobileApp.MVVM.View.DiscoverPage());
    }

    private async void OpenDatabaseBrowser(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DatabaseBrowserPage(Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db")));
    }
}