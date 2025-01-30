using VictuzMobileApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace VictuzMobileApp.MVVM.View
{
	public partial class DiscoverPage : ContentPage
	{
		public ObservableCollection<Activity> FilteredEvents { get; set; } = new ObservableCollection<Activity>();
		private List<Activity> AllEvents { get; set; } = new List<Activity>();

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
				FilteredEvents = new ObservableCollection<Activity>(AllEvents);
				BindingContext = this;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading events: {ex.Message}");
				await DisplayAlert("Error", "Er is een fout opgetreden bij het laden van de evenementen.", "OK");
			}
		}

		private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			string searchText = e.NewTextValue.ToLower();
			FilteredEvents.Clear();

			foreach (var activity in AllEvents.Where(a => a.Name.ToLower().Contains(searchText)))
			{
				FilteredEvents.Add(activity);
			}
		}
	}
}