// Models/MeteorologicalData.cs
using System;
using System.Collections.Generic;
using System.Linq;

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

        public MonthlyMeteoData GetMonthData(int month)
        {
            // Get the monthly data by averaging daily data for the month
            var monthlyData = DailyData
                .Where(d => d.Date.Month == month)
                .ToList();

            if (monthlyData.Count == 0)
                return new MonthlyMeteoData { Month = month };

            return new MonthlyMeteoData
            {
                Month = month,
                GlobHor = monthlyData.Average(d => d.GlobalHorizontalIrradiation),
                DiffHor = monthlyData.Average(d => d.DiffuseHorizontalIrradiation),
                Temperature = monthlyData.Average(d => d.AverageTemperature),
                WindSpeed = monthlyData.Average(d => d.WindSpeed),
                Humidity = monthlyData.Average(d => d.Humidity)
            };
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