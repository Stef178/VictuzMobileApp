using VictuzMobileApp.MVVM.Model;
using System.Linq;

namespace VictuzMobileApp.MVVM.View
{
	public partial class DiscoverPage : ContentPage
	{
		public List<Activity> AllEvents { get; set; }
		public List<Activity> FilteredEvents { get; set; } = new List<Activity>();
		private bool _isAscending = true;

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
				FilterEvents("");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading events: {ex.Message}");
				await DisplayAlert("Error", "Er is een fout opgetreden bij het laden van de evenementen.", "OK");
			}
		}

		private void FilterEvents(string searchText)
		{
			if (string.IsNullOrWhiteSpace(searchText))
			{
				FilteredEvents = AllEvents.ToList();
			}
			else
			{
				FilteredEvents = AllEvents.Where(a => a.Name.ToLower().Trim() == searchText.ToLower().Trim()).ToList();
			}

			SortEvents();
		}

		private void SortEvents()
		{
			if (_isAscending)
			{
				FilteredEvents = FilteredEvents.OrderBy(a => a.StartTime).ToList();
			}
			else
			{
				FilteredEvents = FilteredEvents.OrderByDescending(a => a.StartTime).ToList();
			}
			BindingContext = null;
			BindingContext = this;
		}

		private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			FilterEvents(e.NewTextValue);
		}

		private void OnSortButtonClicked(object sender, EventArgs e)
		{
			_isAscending = !_isAscending;
			SortEvents();
		}

		private async void OnReserveTicketClicked(object sender, EventArgs e)
		{
			if (App.CurrentUser == null)
			{
				await DisplayAlert("Fout", "U moet ingelogd zijn om een ticket te reserveren.", "OK");
				return;
			}

			var selectedActivity = (Activity)((Button)sender).BindingContext;

			if (selectedActivity == null)
			{
				await DisplayAlert("Fout", "Er zijn geen evenementen om te reserveren.", "OK");
				return;
			}

			if (selectedActivity.Participants == null)
			{
				selectedActivity.Participants = new List<Participant>();
			}

			if (selectedActivity.Participants.Count >= selectedActivity.MaxParticipants)
			{
				await DisplayAlert("Vol", "Dit evenement is volgeboekt.", "OK");
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
			selectedActivity.Participants.Add(App.CurrentUser);
			await App.Database.UpdateAsync(selectedActivity);

			await DisplayAlert("Succes", "Uw ticket is gereserveerd en toegevoegd aan uw Wallet!", "OK");
		}

		private async void OnDetailsButtonClicked(object sender, EventArgs e)
		{
			var button = (Button)sender;
			var activity = (Activity)button.BindingContext;

			await Navigation.PushAsync(new ActivityDetailPage(activity));
		}
	}
}
