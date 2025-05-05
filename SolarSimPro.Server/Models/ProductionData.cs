// Models/ProductionData.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class MonthlyProductionData
    {
        public List<ProductionData> MonthlyData { get; set; } = new List<ProductionData>();
        public double TotalAnnualProduction => MonthlyData.Sum(m => m.EGrid);

        public void AddMonth(int month, ProductionData data)
        {
            MonthlyData.Add(data);
        }
    }

    public class ProductionData
    {
        public double GlobHor { get; set; }  // Global horizontal irradiation (kWh/m²)
        public double DiffHor { get; set; }  // Horizontal diffuse irradiation (kWh/m²)
        public double Temperature { get; set; } // Ambient Temperature (°C)
        public double GlobInc { get; set; }  // Global incident in collector plane (kWh/m²)
        public double GlobEff { get; set; }  // Effective Global, corrected for IAM and shadings (kWh/m²)
        public double EArray { get; set; }   // Effective energy at array output (kWh)
        public double EGrid { get; set; }    // Energy injected into grid (kWh)
        public double PerformanceRatio { get; set; } // Performance Ratio
    }
}