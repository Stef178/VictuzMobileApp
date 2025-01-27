using SQLiteBrowser;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp.MVVM.View;


public partial class HomePage : ContentPage
{

	public List<Activity> UpcomingEvents { get; set; }
	public HomePage()
    {
        InitializeComponent();
		LoadUpcomingEvents();
		BindingContext = new VictuzMobileApp.MVVM.ViewModel.HomePageViewModel();
    }

    private async void LoadUpcomingEvents()
    {
        try
        {
            var activities = await App.Database.GetAllAsync<Activity>();

            UpcomingEvents = activities
                .Where(a => a.StartTime >= DateTime.Now)
                .OrderBy(a => a.StartTime)
                .Take(3)
                .ToList();

            BindingContext = this;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading events: {ex.Message}");
            await DisplayAlert("Error", "Er is een fout opgetreden bij het laden van de evenementen.", "OK");
        }
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