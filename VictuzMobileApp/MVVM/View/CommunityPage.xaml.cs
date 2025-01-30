using Microsoft.Maui.Media;
using System.Collections.ObjectModel;

namespace VictuzMobileApp.MVVM.View;

public partial class CommunityPage : ContentPage
{
    public class PhotoModel
    {
        public string ImagePath { get; set; }
        public string Caption { get; set; }
    }

    private ObservableCollection<PhotoModel> _photoList = new ObservableCollection<PhotoModel>();

    public CommunityPage()
    {
        InitializeComponent();

        // Hardcoded voorbeeldfoto's toevoegen
        _photoList.Add(new PhotoModel { ImagePath = "example1.png", Caption = "Eerste voorbeeldfoto" });
        _photoList.Add(new PhotoModel { ImagePath = "example2.png", Caption = "Tweede voorbeeldfoto" });

        // Koppel de lijst aan de UI
        PhotoCollectionView.ItemsSource = _photoList;
    }

    private async void OnAddPhotoClicked(object sender, EventArgs e)
    {
        var photo = await MediaPicker.PickPhotoAsync();
        if (photo != null)
        {
            // Vraag om een onderschrift
            string caption = await DisplayPromptAsync("Foto Toevoegen", "Onderschrift toevoegen:");

            // Zorg dat de gebruiker iets heeft ingevuld
            if (string.IsNullOrWhiteSpace(caption))
            {
                caption = "Geen onderschrift opgegeven";
            }

            _photoList.Add(new PhotoModel
            {
                ImagePath = photo.FullPath,
                Caption = caption
            });

            // UI verversen
            PhotoCollectionView.ItemsSource = null;
            PhotoCollectionView.ItemsSource = _photoList;
        }
    }

}