using System;
using System.Linq;
using VictuzMobileApp.MVVM.Model;
using VictuzMobileApp.MVVM.Data;

namespace VictuzMobileApp.MVVM.View
{
	public partial class DiscoverPage : ContentPage
	{
		public List<Activity> UpcomingEvents { get; set; }

		public DiscoverPage()
		{
			LoadUpcomingEvents();
			InitializeComponent(); 
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
	}
}
