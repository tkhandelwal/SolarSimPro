// Models/SiteAnalysis.cs
public class SiteAnalysis
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public GeoCoordinates Location { get; set; }
    public double Albedo { get; set; } // Ground reflectivity
    public double SiteAltitude { get; set; }
    public TimeZoneInfo TimeZone { get; set; }
    public MeteorologicalData WeatherData { get; set; }
    public List<ShadingObject> NearbyObjects { get; set; }
}