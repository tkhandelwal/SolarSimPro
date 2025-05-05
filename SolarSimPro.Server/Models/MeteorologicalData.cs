// Models/MeteorologicalData.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class MeteorologicalData
    {
        public List<DailyWeatherData> DailyData { get; set; } = new List<DailyWeatherData>();

        public DailyWeatherData GetDailyData(DateTime date)
        {
            return DailyData.FirstOrDefault(d => d.Date.Date == date.Date) ??
                   new DailyWeatherData { Date = date };
        }
    }

    public class DailyWeatherData
    {
        public DateTime Date { get; set; }
        public double GlobalHorizontalIrradiation { get; set; } // kWh/m²
        public double DiffuseHorizontalIrradiation { get; set; } // kWh/m²
        public double DirectNormalIrradiation { get; set; } // kWh/m²
        public double AverageTemperature { get; set; } // °C
        public double MinTemperature { get; set; } // °C
        public double MaxTemperature { get; set; } // °C
        public double WindSpeed { get; set; } // m/s
        public double Humidity { get; set; } // %
        public double Precipitation { get; set; } // mm
        public double IncidentIrradiation { get; set; } // kWh/m²
        public double EffectiveIrradiation { get; set; } // kWh/m²
    }
}