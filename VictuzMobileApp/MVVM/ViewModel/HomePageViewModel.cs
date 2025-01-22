using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace VictuzMobileApp.MVVM.ViewModel
{
    public class MainPageViewModel
    {
        // Navigatie Command
        public ICommand NavigateCommand { get; }

        public MainPageViewModel()
        {
            // Koppel het navigatiecommando
            NavigateCommand = new Command<string>(Navigate);
        }

        private async void Navigate(string pageName)
        {
            // Switch voor verschillende pagina's
            Page page = pageName switch
            {
                "DiscoverPage" => new VictuzMobileApp.MVVM.View.DiscoverPage(),
                "SearchPage" => new VictuzMobileApp.MVVM.View.SearchPage(),
                "CommunityPage" => new VictuzMobileApp.MVVM.View.CommunityPage(),
                "WalletPage" => new VictuzMobileApp.MVVM.View.WalletPage(),
                "SettingsPage" => new VictuzMobileApp.MVVM.View.SettingsPage(),
                _ => null
            };

            if (page != null)
            {
                // Zorg dat er een NavigationPage is ingesteld in App.xaml.cs
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }
        }
    }
}
