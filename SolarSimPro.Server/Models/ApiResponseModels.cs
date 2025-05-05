// Models/ApiResponseModels.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class NrelSolarDataResponse
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public List<NrelMonthlyData> MonthlyData { get; set; } = new List<NrelMonthlyData>();
    }

    public class NrelMonthlyData
    {
        public int Month { get; set; }
        public double GHI { get; set; }  // Global Horizontal Irradiance
        public double DHI { get; set; }  // Diffuse Horizontal Irradiance
        public double DNI { get; set; }  // Direct Normal Irradiance
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
    }

    public class GoogleGeocodingResponse
    {
        public List<GoogleGeocodingResult> Results { get; set; } = new List<GoogleGeocodingResult>();
    }

    public class GoogleGeocodingResult
    {
        public string FormattedAddress { get; set; } = string.Empty;
        public GoogleGeometry Geometry { get; set; } = new GoogleGeometry();
    }

    public class GoogleGeometry
    {
        public GoogleLocation Location { get; set; } = new GoogleLocation();
    }

    public class GoogleLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class GoogleTimeZoneResponse
    {
        public string TimeZoneId { get; set; } = string.Empty;
        public string TimeZoneName { get; set; } = string.Empty;
    }
}