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

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
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
                UserName = currentUser.Name;
                FirstName = currentUser.Name.Split(' ')[0];
                LastName = currentUser.Name.Contains(" ") ? currentUser.Name.Split(' ')[1] : "";
                BirthDate = currentUser.BirthDate;
                City = currentUser.City;
                Country = currentUser.Country;

                // Controleer of er een profielfoto is ingesteld, zo niet, gebruik person.png
                ProfileImage = string.IsNullOrEmpty(currentUser.ProfilePicturePath)
                    ? "person.png"
                    : currentUser.ProfilePicturePath;
            }
        }


        private async Task SaveProfile()
        {
            try
            {
                var currentUser = App.CurrentUser;
                if (currentUser != null)
                {
                    currentUser.Name = UserName;
                    currentUser.ProfilePicturePath = ProfileImage;
                    OnPropertyChanged(nameof(ProfileImage));
                    currentUser.BirthDate = BirthDate;
                    currentUser.City = City;
                    currentUser.Country = Country;

                    await App.Database.UpdateAsync(currentUser);

                    // Notify HomePage dat de data is gewijzigd
                    OnPropertyChanged(nameof(App.CurrentUser.ProfilePicturePath));

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

                    ProfileImage = localPath;
                    App.CurrentUser.ProfilePicturePath = localPath;

                    await App.Database.UpdateAsync(App.CurrentUser);

                    // 🔹 Zorg ervoor dat de UI wordt bijgewerkt
                    OnPropertyChanged(nameof(ProfileImage));
                    OnPropertyChanged(nameof(App.CurrentUser));
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

                    ProfileImage = localPath;
                    App.CurrentUser.ProfilePicturePath = localPath;

                    await App.Database.UpdateAsync(App.CurrentUser);

                    // Forceer UI-update
                    OnPropertyChanged(nameof(ProfileImage));
                    OnPropertyChanged(nameof(App.CurrentUser.ProfilePicturePath));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er ging iets mis: {ex.Message}", "OK");
            }
        }

    }
}
