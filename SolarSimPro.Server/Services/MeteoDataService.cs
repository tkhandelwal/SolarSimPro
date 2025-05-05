// Services/MeteoDataService.cs
using System.Threading.Tasks;
using SolarSimPro.Server.Services.Interfaces;
using SolarSimPro.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace SolarSimPro.Server.Services
{
    public class MeteoDataService : IMeteoDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MeteoDataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<MeteoData> GetMeteoDataAsync(double latitude, double longitude)
        {
            // You can implement this to fetch from a real API like NREL or SolarGIS
            // For now, we'll return mock data

            var meteoData = new MeteoData
            {
                Latitude = latitude,
                Longitude = longitude,
                Elevation = 100,
                MonthlyData = GenerateMockMonthlyData()
            };

            return meteoData;
        }

        private List<MonthlyMeteoData> GenerateMockMonthlyData()
        {
            // Generate mock monthly meteorological data based on PVsyst report
            var monthlyData = new List<MonthlyMeteoData>();

            double[] globHorValues = { 125.9, 138.6, 167.1, 195.6, 226.9, 222.9, 205.8, 200.9, 185.4, 173.6, 138.9, 116.9 };
            double[] diffHorValues = { 32.55, 34.44, 52.39, 55.20, 58.28, 59.10, 67.58, 60.76, 47.70, 35.96, 28.20, 30.69 };
            double[] tempValues = { 19.99, 20.85, 23.60, 27.67, 32.00, 33.90, 35.11, 34.81, 32.80, 29.46, 25.61, 21.92 };

            for (int month = 1; month <= 12; month++)
            {
                monthlyData.Add(new MonthlyMeteoData
                {
                    Month = month,
                    GlobHor = globHorValues[month - 1],
                    DiffHor = diffHorValues[month - 1],
                    Temperature = tempValues[month - 1],
                    WindSpeed = 3.5, // Mock value
                    Humidity = 60 // Mock value
                });
            }

            return monthlyData;
        }
    }
}