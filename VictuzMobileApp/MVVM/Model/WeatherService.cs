using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace VictuzMobileApp.MVVM.Model
{
    public class WeatherService
    {
        private const string ApiKey = "67f1e0e95abc55499524a1c20f4d8894";
        private const string ApiUrl = "https://api.openweathermap.org/data/2.5/weather?q=Heerlen&appid=" + ApiKey + "&units=metric";

        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherResponse> GetWeatherAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(ApiUrl);
                var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(response);
                return weatherData;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error fetching weather data: {ex.Message}");
                return null;
            }
        }
    }

    public class WeatherResponse
    {
        public MainWeather Main { get; set; }
        public string Name { get; set; }
        public string WeatherDescription => Main != null ? $"{Main.Temp}°C" : "Geen gegevens beschikbaar";
    }

    public class MainWeather
    {
        public double Temp { get; set; }
    }
}
