using Plugin.Media;
using Plugin.Media.Abstractions;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp.MVVM.View
{
    public partial class CreateActivityPage : ContentPage
    {
        public Activity SelectedActivity { get; set; }

        public CreateActivityPage()
        {
            InitializeComponent();

            SelectedActivity = new Activity();
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

        private async void OnSelectPhotoButtonClicked(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 75
            });

            if (photo != null)
            {
                ActivityImage.Source = ImageSource.FromFile(photo.Path);

                SelectedActivity.PhotoPath = photo.Path;
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

            DateTime startDateTime = StartDatePicker.Date.Add(StartTimePicker.Time);
            DateTime endDateTime = EndDatePicker.Date.Add(EndTimePicker.Time);

            if (endDateTime <= startDateTime)
            {
                await DisplayAlert("Fout", "De einddatum moet na de startdatum liggen.", "OK");
                return;
            }

            var newActivity = new Activity
            {
                Name = NameEntry.Text,
                Category = SelectedCategoryType,
                Description = DescriptionEntry.Text,
                StartTime = startDateTime,
                EndTime = endDateTime,
                MaxParticipants = maxParticipants,
                PhotoPath = SelectedActivity.PhotoPath 
            };

            await App.Database.AddAsync(newActivity);
            await DisplayAlert("Succes", "Activiteit succesvol aangemaakt!", "OK");
            await Navigation.PopAsync();
        }
    }
}
