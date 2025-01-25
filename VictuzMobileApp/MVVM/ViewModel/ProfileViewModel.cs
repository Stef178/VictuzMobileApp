using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp.MVVM.ViewModel
{
    public class ProfileViewModel : BindableObject
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _birthDate;
        public string BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged();
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        private string _profileImage;
        public string ProfileImage
        {
            get => _profileImage;
            set
            {
                _profileImage = value;
                OnPropertyChanged();
            }
        }

        public bool IsMale { get; set; }
        public bool IsFemale => !IsMale;

        public ICommand SaveProfileCommand { get; }
        public ICommand ShowPhotoMenuCommand { get; }

        public ProfileViewModel()
        {
            SaveProfileCommand = new Command(async () => await SaveProfile());
            ShowPhotoMenuCommand = new Command(async () => await ShowPhotoMenu());

            // Laad gegevens van de huidige gebruiker
            LoadUserData();
        }

        private void LoadUserData()
        {
            var currentUser = App.CurrentUser;
            if (currentUser != null)
            {
                // Vul de ViewModel-gegevens met de huidige gebruiker
                var nameParts = currentUser.Name.Split(' ');
                FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
                LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
                BirthDate = currentUser.BirthDate;
                City = currentUser.City;
                Country = currentUser.Country;
                ProfileImage = string.IsNullOrEmpty(currentUser.ProfilePicturePath)
                    ? "person.png" // Standaard afbeelding
                    : currentUser.ProfilePicturePath;
            }
        }

        private async Task SaveProfile()
        {
            // Update de huidige gebruiker in de database
            try
            {
                var currentUser = App.CurrentUser;
                if (currentUser != null)
                {
                    currentUser.Name = $"{FirstName} {LastName}";
                    currentUser.BirthDate = BirthDate;
                    currentUser.City = City;
                    currentUser.Country = Country;
                    currentUser.ProfilePicturePath = ProfileImage;

                    await App.Database.UpdateAsync(currentUser);
                    await Application.Current.MainPage.DisplayAlert("Succes", "Profiel is bijgewerkt!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er ging iets mis: {ex.Message}", "OK");
            }
        }

        private async Task ShowPhotoMenu()
        {
            string action = await Application.Current.MainPage.DisplayActionSheet(
                "Profielfoto",
                "Annuleren",
                null,
                "Kies een foto uit de galerij",
                "Maak een nieuwe profielfoto");

            switch (action)
            {
                case "Kies een foto uit de galerij":
                    await PickPhotoFromGallery();
                    break;
                case "Maak een nieuwe profielfoto":
                    await TakeNewPhoto();
                    break;
            }
        }

        private async Task PickPhotoFromGallery()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    string localPath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                    using var stream = await photo.OpenReadAsync();
                    using var fileStream = File.Create(localPath);
                    await stream.CopyToAsync(fileStream);

                    ProfileImage = localPath; // Update de binding
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er ging iets mis: {ex.Message}", "OK");
            }
        }

        private async Task TakeNewPhoto()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    string localPath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                    using var stream = await photo.OpenReadAsync();
                    using var fileStream = File.Create(localPath);
                    await stream.CopyToAsync(fileStream);

                    ProfileImage = localPath; // Update de binding
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er ging iets mis: {ex.Message}", "OK");
            }
        }
    }
}
