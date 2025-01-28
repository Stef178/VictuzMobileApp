using VictuzMobileApp.Services;
using VictuzMobileApp.MVVM.Model;
using SQLiteBrowser;

namespace VictuzMobileApp.MVVM.View
{
    public partial class HomePage : ContentPage
    {
        private readonly WeatherService _weatherService;

        public List<Activity> UpcomingEvents { get; set; }

        public HomePage()
        {
            InitializeComponent();
            _weatherService = new WeatherService(); // Initialize WeatherService
            LoadUpcomingEvents();
        }

        private async void LoadUpcomingEvents()
        {
            try
            {
                var activities = await App.Database.GetAllAsync<Activity>();
                UpcomingEvents = activities
                    .Where(a => a.StartTime >= DateTime.Now)
                    .OrderBy(a => a.StartTime)
                    .Take(3)
                    .ToList();

                BindingContext = this;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading events: {ex.Message}");
                await DisplayAlert("Error", "Er is een fout opgetreden bij het laden van de evenementen.", "OK");
            }
        }

        // Method to load weather info
        private async Task LoadWeather()
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(); // Get weather data for Heerlen

                if (weather != null)
                {
                    // Update UI elements directly
                    WeatherDescriptionLabel.Text = $"Weer: {weather.Weather[0].Description}";
                    TemperatureLabel.Text = $"Temperatuur: {weather.Main.Temp} °C";
                    HumidityLabel.Text = $"Luchtvochtigheid: {weather.Main.Humidity} %";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading weather: {ex.Message}");
                await DisplayAlert("Error", "Er is een fout opgetreden bij het laden van het weer.", "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.CurrentUser != null && string.IsNullOrEmpty(App.CurrentUser.ProfilePicturePath))
            {
                App.CurrentUser.ProfilePicturePath = "person.png";
            }

            await LoadWeather();  // Load weather data when the page appears
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            bool confirmLogout = await DisplayAlert("Uitloggen", "Weet u zeker dat u wilt uitloggen?", "Ja", "Nee");
            if (!confirmLogout)
                return;

            try
            {
                await App.Database.SetActiveUser(0);
                App.CurrentUser = null;

                await Navigation.PushAsync(new StartPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", $"Er is iets misgegaan tijdens het uitloggen: {ex.Message}", "OK");
            }
        }

        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void OnDiscoverButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VictuzMobileApp.MVVM.View.DiscoverPage());
        }

        private async void OpenDatabaseBrowser(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DatabaseBrowserPage(Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db")));
        }
    }
}
