using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;

namespace VictuzMobileApp.MVVM.ViewModel
{
    public class ProfileViewModel : BindableObject
    {
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

        public ICommand ShowPhotoMenuCommand { get; }

        public ProfileViewModel()
        {
            // Standaard afbeelding instellen
            ProfileImage = "person.png";

            // Command voor het tonen van het menu
            ShowPhotoMenuCommand = new Command(async () => await ShowPhotoMenu());
        }

        private async Task ShowPhotoMenu()
        {
            // Toon een actieblad met opties
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
                // Foto kiezen uit galerij
                var photo = await MediaPicker.PickPhotoAsync();

                if (photo != null)
                {
                    string localPath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                    // Foto lokaal opslaan
                    using var stream = await photo.OpenReadAsync();
                    using var fileStream = File.Create(localPath);
                    await stream.CopyToAsync(fileStream);

                    // Profielfoto bijwerken
                    UpdateProfilePicture(localPath);
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
                // Gebruik de camera om een foto te maken
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    string localPath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                    // Sla de foto lokaal op
                    using var stream = await photo.OpenReadAsync();
                    using var fileStream = File.Create(localPath);
                    await stream.CopyToAsync(fileStream);

                    // Profielfoto bijwerken
                    UpdateProfilePicture(localPath);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er ging iets mis: {ex.Message}", "OK");
            }
        }

        private void UpdateProfilePicture(string photoPath)
        {
            // Update de UI door de nieuwe profielfoto in de binding te zetten
            ProfileImage = photoPath;

            // Logica om de nieuwe profielfoto in de database op te slaan (optioneel)
        }
    }
}
