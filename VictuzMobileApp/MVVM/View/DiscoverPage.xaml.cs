using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp.MVVM.View
{
	public partial class DiscoverPage : ContentPage
	{
		public List<Activity> AllEvents { get; set; }

		public DiscoverPage()
		{
			InitializeComponent();
			LoadAllEvents();
		}

		private async void LoadAllEvents()
		{
			try
			{
				var activities = await App.Database.GetAllAsync<Activity>();

				AllEvents = activities.OrderBy(a => a.StartTime).ToList();

				BindingContext = this;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading events: {ex.Message}");
				await DisplayAlert("Error", "Er is een fout opgetreden bij het laden van de evenementen.", "OK");
			}
		}
	}
}
