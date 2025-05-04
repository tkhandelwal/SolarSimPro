// Models/Project.cs
public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ClientName { get; set; }
    public string Location { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public string TimeZone { get; set; }
    public double Albedo { get; set; } = 0.20; // Default like in PVsyst
    public ProjectType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public List<SolarSystem> Systems { get; set; } = new List<SolarSystem>();
    public List<ShadingObject> ShadingObjects { get; set; } = new List<ShadingObject>();
    public MeteoData MeteoData { get; set; }
}

public enum ProjectType
{
    Residential,
    Commercial,
    Industrial,
    Utility
}
