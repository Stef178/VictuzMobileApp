using SQLiteBrowser;
using VictuzMobileApp.MVVM.Model;
using System.Collections.ObjectModel;

namespace VictuzMobileApp.MVVM.View;

public partial class HomePage : ContentPage
{
	public ObservableCollection<Activity> UpcomingEvents { get; set; } = new ObservableCollection<Activity>();

	public HomePage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	public async Task LoadUpcomingEvents()
	{
		try
		{
			UpcomingEvents.Clear();

			var activities = (await App.Database.GetAllAsync<Activity>()).ToList();

			var upcomingActivities = activities
				.Where(a => a.StartTime.Date >= DateTime.Now.Date) 
				.OrderBy(a => a.StartTime.Date) 
				.ThenBy(a => a.StartTime.TimeOfDay)  
				.Take(3)  
				.ToList();

			foreach (var activity in upcomingActivities)
			{
				UpcomingEvents.Add(activity);

				Console.WriteLine($"Added activity: {activity.Name} on {activity.StartTime}");
			}

			Console.WriteLine($"Total upcoming activities loaded: {upcomingActivities.Count}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error loading events: {ex.Message}");
			await DisplayAlert("Error", "Er is een fout opgetreden bij het laden van de evenementen.", "OK");
		}
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadUpcomingEvents();
		if (App.CurrentUser != null && string.IsNullOrEmpty(App.CurrentUser.ProfilePicturePath))
		{
			App.CurrentUser.ProfilePicturePath = "person.png";
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
    private async void OnReserveTicketClicked(object sender, EventArgs e)
    {
        if (App.CurrentUser == null)
        {
            await DisplayAlert("Fout", "U moet ingelogd zijn om een ticket te reserveren.", "OK");
            return;
        }

        var selectedActivity = UpcomingEvents.FirstOrDefault(); 
        if (selectedActivity == null)
        {
            await DisplayAlert("Fout", "Er zijn geen evenementen om te reserveren.", "OK");
            return;
        }

        var ticket = new Ticket
        {
            ParticipantId = App.CurrentUser.Id,
            ActivityId = selectedActivity.Id,
            Price = 0, 
            IsPaid = false
        };

        await App.Database.AddAsync(ticket);
        await DisplayAlert("Succes", "Uw ticket is gereserveerd en toegevoegd aan uw Wallet!", "OK");
    }

    private async void OnDetailsButtonClicked(object sender, EventArgs e)
    {
        
        var button = (Button)sender;
        var activity = (Activity)button.BindingContext;

       
        await Navigation.PushAsync(new ActivityDetailPage(activity));
    }


}