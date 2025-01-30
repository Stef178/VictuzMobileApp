using Microsoft.Maui.Media;
using System.Collections.ObjectModel;
using Microsoft.Maui.Media;
using System.Collections.ObjectModel;

namespace VictuzMobileApp.MVVM.View
{
    public partial class CommunityPage : ContentPage
    {
        private ObservableCollection<PhotoModel> _photoList = new ObservableCollection<PhotoModel>();

        
        private DatabaseService _dbService;

        public CommunityPage()
        {
            InitializeComponent();

            
            _dbService = new DatabaseService();

            
            var savedPhotos = _dbService.GetAllPhotos();

            
            foreach (var photo in savedPhotos)
            {
                _photoList.Add(photo);
            }

            
            PhotoCollectionView.ItemsSource = _photoList;
        }

        private async void OnAddPhotoClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                
                string caption = await DisplayPromptAsync("Foto Toevoegen", "Onderschrift toevoegen:");

                if (string.IsNullOrWhiteSpace(caption))
                {
                    caption = "Geen onderschrift opgegeven";
                }

                var newPhoto = new PhotoModel
                {
                    ImagePath = photo.FullPath,
                    Caption = caption
                };

                _photoList.Add(newPhoto);

                _dbService.AddPhoto(newPhoto);

                PhotoCollectionView.ItemsSource = null;
                PhotoCollectionView.ItemsSource = _photoList;
            }
        }
    }
}
