using VictuzMobileApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace VictuzMobileApp.MVVM.View
{
	public partial class DiscoverPage : ContentPage
	{
		public ObservableCollection<Activity> FilteredEvents { get; set; } = new ObservableCollection<Activity>();
		private List<Activity> AllEvents { get; set; } = new List<Activity>();
		private bool SortAscending = true;

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
				UpdateFilteredEvents();
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
			UpdateFilteredEvents(e.NewTextValue);
		}

		private void OnSortButtonClicked(object sender, EventArgs e)
		{
			SortAscending = !SortAscending;
			UpdateFilteredEvents();
		}

		private void UpdateFilteredEvents(string searchText = "")
		{
			searchText = searchText?.ToLower() ?? string.Empty;
			var filtered = AllEvents.Where(a => a.Name.ToLower().Contains(searchText));

			if (SortAscending)
			{
				filtered = filtered.OrderBy(a => a.StartTime);
			}
			else
			{
				filtered = filtered.OrderByDescending(a => a.StartTime);
			}

			FilteredEvents.Clear();
			foreach (var activity in filtered)
			{
				FilteredEvents.Add(activity);
			}
		}
	}
}
