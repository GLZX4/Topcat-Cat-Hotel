using System.Text.Json;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "8188f18ddbf5c2eaba1172b10b8287eb"; // Your API key
    private readonly string _apiUrl = "https://api.openweathermap.org/data/2.5/weather?q=Urmston&appid=8188f18ddbf5c2eaba1172b10b8287eb";

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetWeatherConditionAsync()
    {
        var response = await _httpClient.GetStringAsync(_apiUrl);
        var weatherData = JsonDocument.Parse(response);

        var weather = weatherData.RootElement.GetProperty("weather")[0].GetProperty("main").GetString();
        return weather;
    }
}
