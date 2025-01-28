using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace VictuzMobileApp.Services
{
    public class WeatherService
    {
        private const string ApiKey = "2e75ccd6dbbd41a3b85f28253b812b74"; // Voeg hier je API-sleutel toe
        private const string BaseUrl = "http://api.openweathermap.org/data/2.5/weather?q=Heerlen&appid=2e75ccd6dbbd41a3b85f28253b812b74&units=metric\r\n"; // Basis-URL van de OpenWeatherMap API

        public async Task<WeatherResponse> GetWeatherAsync()
        {
            string cityId = "2754210"; // ID voor Heerlen
            using (HttpClient client = new HttpClient())
            {
                var url = $"{BaseUrl}?id={cityId}&appid={ApiKey}&units=metric"; // Voeg units=metric toe voor Celsius
                try
                {
                    var response = await client.GetStringAsync(url);
                    var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(response);
                    return weatherData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving weather data: {ex.Message}");
                    return null;
                }
            }
        }
    }

    public class WeatherResponse
    {
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public double Humidity { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }
}
