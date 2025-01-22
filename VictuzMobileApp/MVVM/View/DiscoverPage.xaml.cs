using System;
using Microsoft.Maui.Controls;

namespace VictuzMobileApp.MVVM.View
{
    public partial class DiscoverPage : ContentPage
    {
        public DiscoverPage()
        {
            InitializeComponent();
        }

        private async void OnActivityTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActivityDetailPage());
        }

        private async void OnWinGamesButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WinGamesPage());
        }
    }
}
