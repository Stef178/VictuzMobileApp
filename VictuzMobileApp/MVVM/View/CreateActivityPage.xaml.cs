using System;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp.MVVM.View
{
    public partial class CreateActivityPage : ContentPage
    {
        public CreateActivityPage()
        {
            InitializeComponent(); 
        }

		public string SelectedCategoryType { get; set; }


		private void OnCategoryTypeChanged(object sender, EventArgs e)
		{
			if (CategoryTypePicker.SelectedItem != null)
			{
				SelectedCategoryType = CategoryTypePicker.SelectedItem.ToString();
				Console.WriteLine($"Geselecteerde categorie: {SelectedCategoryType}");
			}
			else
			{
				Console.WriteLine("Geen categorie geselecteerd.");
			}
		}


		private async void OnCreateActivityButtonClicked(object sender, EventArgs e)
        {
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Fout", "Je moet ingelogd zijn om een activiteit aan te maken.", "OK");
                return;
            }

            if (App.CurrentUser.Email != App.AdminUser.Email)
            {
                await DisplayAlert("Fout", "Alleen de admin kan activiteiten aanmaken.", "OK");
                return;
            }

			int maxParticipants;
			bool isMaxParticipantsValid = int.TryParse(MaxParticipantsEntry.Text, out maxParticipants);

			int maxLimit = 500;

			if (!isMaxParticipantsValid || maxParticipants <= 0 || maxParticipants > maxLimit)
			{
				await DisplayAlert("Fout", $"Het aantal deelnemers mag niet groter zijn dan {maxLimit}.", "OK");
				return;
			}

			var newActivity = new Activity
            {
                Name = NameEntry.Text,         
                Category = SelectedCategoryType,
				Description = DescriptionEntry.Text,
                StartTime = StartDatePicker.Date,
                EndTime = EndDatePicker.Date,
				MaxParticipants = maxParticipants
			};

            await App.Database.AddAsync(newActivity);
            await DisplayAlert("Succes", "Activiteit succesvol aangemaakt!", "OK");
            await Navigation.PopAsync();
        }
    }
}
