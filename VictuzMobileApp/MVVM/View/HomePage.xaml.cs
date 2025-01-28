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

			// Haal alle activiteiten op
			var activities = (await App.Database.GetAllAsync<Activity>()).ToList();

			// Filter op huidige datum en tijd, sorteer op startdatum/tijd, en pak de eerste 3
			var upcomingActivities = activities
				.Where(a => a.StartTime.Date >= DateTime.Now.Date) // Vergelijk alleen de datums
				.OrderBy(a => a.StartTime.Date)  // Sorteer eerst op datum
				.ThenBy(a => a.StartTime.TimeOfDay)  // Dan op tijd binnen dezelfde datum
				.Take(3)  // Pak de eerste 3
				.ToList();

			// Voeg ze toe aan de ObservableCollection
			foreach (var activity in upcomingActivities)
			{
				UpcomingEvents.Add(activity);
				// Debug logging
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

	// Rest van de code blijft hetzelfde...
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
}