// Models/GeographicModels.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class GeoCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }

    public class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FormattedAddress { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
    }
}