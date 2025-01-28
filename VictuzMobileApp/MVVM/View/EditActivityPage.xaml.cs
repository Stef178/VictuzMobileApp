using SQLite;
using VictuzMobileApp.MVVM.Model;
using System;

namespace VictuzMobileApp.MVVM.View
{
	public partial class EditActivityPage : ContentPage
	{
		private string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db");
		public Activity CurrentActivity { get; set; }
		public string SelectedCategoryType { get; set; }

		public EditActivityPage(Activity activity)
		{
			InitializeComponent();
			CurrentActivity = activity;
			BindingContext = this;

			NameEntry.Text = CurrentActivity.Name;
			CategoryTypePicker.SelectedItem = CurrentActivity.Category;
			DescriptionEntry.Text = CurrentActivity.Description;
			StartDatePicker.Date = CurrentActivity.StartTime.Date;
			StartTimePicker.Time = CurrentActivity.StartTime.TimeOfDay;
			EndDatePicker.Date = CurrentActivity.EndTime.Date;
			EndTimePicker.Time = CurrentActivity.EndTime.TimeOfDay;
			MaxParticipantsEntry.Text = CurrentActivity.MaxParticipants.ToString();
		}

		private void OnCategoryTypeChanged(object sender, EventArgs e)
		{
			if (CategoryTypePicker.SelectedItem != null)
			{
				SelectedCategoryType = CategoryTypePicker.SelectedItem.ToString();
			}
		}

		private async void OnSaveButtonClicked(object sender, EventArgs e)
		{
			int maxParticipants;
			bool isMaxParticipantsValid = int.TryParse(MaxParticipantsEntry.Text, out maxParticipants);
			int maxLimit = 500;

			if (!isMaxParticipantsValid || maxParticipants <= 0 || maxParticipants > maxLimit)
			{
				await DisplayAlert("Fout", $"Het aantal deelnemers mag niet groter zijn dan {maxLimit}.", "OK");
				return;
			}

			DateTime startDateTime = StartDatePicker.Date.Add(StartTimePicker.Time);
			DateTime endDateTime = EndDatePicker.Date.Add(EndTimePicker.Time);

			if (endDateTime <= startDateTime)
			{
				await DisplayAlert("Fout", "De einddatum moet na de startdatum liggen.", "OK");
				return;
			}

			CurrentActivity.Name = NameEntry.Text;
			CurrentActivity.Category = SelectedCategoryType;
			CurrentActivity.Description = DescriptionEntry.Text;
			CurrentActivity.StartTime = startDateTime;
			CurrentActivity.EndTime = endDateTime;
			CurrentActivity.MaxParticipants = maxParticipants;

			using (var db = new SQLiteConnection(_dbPath))
			{
				db.CreateTable<Activity>(); 
				db.Update(CurrentActivity);
			}

			await DisplayAlert("Succes", "Activiteit succesvol bijgewerkt!", "OK");
			await Navigation.PopAsync(); 
		}
	}
}
