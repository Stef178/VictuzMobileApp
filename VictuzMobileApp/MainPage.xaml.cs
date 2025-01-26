using SQLiteBrowser;
using System.IO;

namespace VictuzMobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCreateActivityClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VictuzMobileApp.MVVM.View.CreateActivityPage());
        }

        private async void OpenDatabaseBrowser(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DatabaseBrowserPage(Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db")));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // ✅ Maak de huidige gebruiker "inactief"
            App.CurrentUser = null;

            // ✅ Keer terug naar de LoginPage
            Application.Current.MainPage = new NavigationPage(new VictuzMobileApp.MVVM.View.StartPage());
        }
    }
}
