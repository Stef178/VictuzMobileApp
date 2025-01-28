using SQLite;
using VictuzMobileApp.MVVM.Model;
using System.Collections.ObjectModel;

namespace VictuzMobileApp.MVVM.View
{
	public partial class ManageActivityPage : ContentPage
	{
		public ObservableCollection<Activity> Activities { get; set; }

		private string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db");

		public ManageActivityPage()
		{
			InitializeComponent();

			Activities = new ObservableCollection<Activity>();
			BindingContext = this;

			LoadActivities();
		}

		private void LoadActivities()
		{
			using (var db = new SQLiteConnection(_dbPath))
			{
				db.CreateTable<Activity>(); 

				var activitiesFromDb = db.Table<Activity>().ToList();

				Activities.Clear();
				foreach (var activity in activitiesFromDb)
				{
					Activities.Add(activity);
				}
			}
		}

		private async void EditActivity(object sender, EventArgs e)
		{
			var button = sender as Button;
			var activity = button?.BindingContext as Activity;

			if (activity != null)
			{
				await Navigation.PushAsync(new EditActivityPage(activity));
			}
		}


		private async void DeleteActivity(object sender, EventArgs e)
		{
			var button = sender as Button;
			var activity = button?.BindingContext as Activity;

			if (activity != null)
			{
				bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete: {activity.Name}?", "Yes", "No");

				if (confirm)
				{
					using (var db = new SQLiteConnection(_dbPath))
					{
						db.Delete(activity); 
					}

					Activities.Remove(activity);

					await DisplayAlert("Deleted", $"{activity.Name} has been deleted.", "OK");
				}
			}
		}
	}

}
