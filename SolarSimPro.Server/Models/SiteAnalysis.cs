// Models/SiteAnalysis.cs
using System;
using System.Collections.Generic;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Models
{
    public class SiteAnalysis
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public GeoCoordinates Location { get; set; } = new GeoCoordinates();
        public double Albedo { get; set; } // Ground reflectivity
        public double SiteAltitude { get; set; }
        public string TimeZone { get; set; } = string.Empty;
        public MeteorologicalData WeatherData { get; set; } = new MeteorologicalData();
        public List<ShadingObject> NearbyObjects { get; set; } = new List<ShadingObject>();
    }
}