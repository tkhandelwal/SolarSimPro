// Services/LocationService.cs
public class LocationService : ILocationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public LocationService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["MapApiKey"];
    }

    public async Task<GeoLocation> GetLocationDetailsAsync(string address)
    {
        // Use Google Maps Geocoding API or similar to get coordinates from address
        var response = await _httpClient.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<GoogleGeocodingResponse>();

        if (content.Results.Count == 0)
            throw new KeyNotFoundException("Location not found");

        var result = content.Results[0];
        var location = result.Geometry.Location;

        return new GeoLocation
        {
            Latitude = location.Lat,
            Longitude = location.Lng,
            FormattedAddress = result.FormattedAddress,
            // Set time zone - could make another API call to get this
            TimeZone = await GetTimeZoneAsync(location.Lat, location.Lng)
        };
    }

    public async Task<string> GetTimeZoneAsync(double latitude, double longitude)
    {
        // Use Google Time Zone API to get time zone for coordinates
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var response = await _httpClient.GetAsync($"https://maps.googleapis.com/maps/api/timezone/json?location={latitude},{longitude}&timestamp={timestamp}&key={_apiKey}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<GoogleTimeZoneResponse>();
        return content.TimeZoneId;
    }

    public async Task<MeteoData> GetMeteoDataAsync(double latitude, double longitude)
    {
        // Use a solar meteorological data API like NREL or SolarGIS
        // This is a simplified version - you'd need to integrate with an actual API

        var response = await _httpClient.GetAsync($"https://developer.nrel.gov/api/solar/nsrdb_data_query.json?api_key={_apiKey}&lat={latitude}&lon={longitude}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<NrelSolarDataResponse>();

        return MapToMeteoData(content);
    }

    private MeteoData MapToMeteoData(NrelSolarDataResponse response)
    {
        // Map the API response to your MeteoData model
        // This would extract monthly averages of solar irradiation, temperature, etc.

        var meteoData = new MeteoData
        {
            Latitude = response.Latitude,
            Longitude = response.Longitude,
            Elevation = response.Elevation,
            MonthlyData = new List<MonthlyMeteoData>()
        };

        // Process the monthly data
        foreach (var monthData in response.MonthlyData)
        {
            meteoData.MonthlyData.Add(new MonthlyMeteoData
            {
                Month = monthData.Month,
                GlobHor = monthData.GHI,
                DiffHor = monthData.DHI,
                Temperature = monthData.Temperature,
                WindSpeed = monthData.WindSpeed,
                // Other meteorological data
            });
        }

        return meteoData;
    }
}