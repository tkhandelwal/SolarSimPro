// Models/MeteoData.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class MeteoData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public List<MonthlyMeteoData> MonthlyData { get; set; } = new List<MonthlyMeteoData>();
    }

    public class MonthlyMeteoData
    {
        public int Month { get; set; }
        public double GlobHor { get; set; }  // Global horizontal irradiation
        public double DiffHor { get; set; }  // Diffuse horizontal irradiation
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double Humidity { get; set; }

        public double GetDailyIrradiation(int day)
        {
            // Simplified: return the same value for each day
            return GlobHor / DateTime.DaysInMonth(DateTime.Now.Year, Month);
        }
    }
}