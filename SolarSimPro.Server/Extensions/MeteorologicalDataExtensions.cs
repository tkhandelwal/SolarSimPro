// SolarSimPro.Server/Extensions/MeteorologicalDataExtensions.cs
using SolarSimPro.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarSimPro.Server.Extensions
{
    public static class MeteorologicalDataExtensions
    {
        public static List<DailyWeatherData> GetMonthData(this MeteorologicalData meteoData, int month)
        {
            return meteoData.DailyData
                .Where(d => d.Date.Month == month)
                .ToList();
        }
    }
}