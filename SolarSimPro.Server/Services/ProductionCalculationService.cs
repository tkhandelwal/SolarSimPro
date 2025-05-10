// Services/ProductionCalculationService.cs
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarSimPro.Server.Services
{
    public class ProductionCalculationService
    {
        // Add this method to your class
        private double ApplyInverterEfficiency(double dcEnergy, List<Inverter> inverters)
        {
            // If no inverters, assume a default efficiency
            if (inverters == null || inverters.Count == 0)
            {
                // Using a typical average efficiency of 97%
                return dcEnergy * 0.97;
            }

            // If multiple inverters, calculate weighted average efficiency
            double totalDcPower = 0;
            double totalAcPower = 0;

            foreach (var inverter in inverters)
            {
                double inputPower = dcEnergy / inverters.Count; // Simple distribution
                double efficiency = inverter.InverterModel?.MaxEfficiency ?? 0.97; // Default to 97% if not specified

                totalDcPower += inputPower;
                totalAcPower += inputPower * efficiency;
            }

            return totalAcPower;
        }

        public MonthlyProductionData CalculateProduction(SolarSystem system, SiteAnalysis site, ShadingResult shading)
        {
            var result = new MonthlyProductionData();

            // For each month
            for (int month = 1; month <= 12; month++)
            {
                // Get meteorological data for this month
                var meteoData = site.WeatherData.GetMonthData(month);

                double monthlyEnergy = 0;

                // Calculate for each day of the month (or use average day)
                int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);

                for (int day = 1; day <= daysInMonth; day++)
                {
                    // Get daily irradiation data
                    var dailyIrradiation = meteoData.GetDailyIrradiation(day);

                    // Additional calculation logic would be here
                    // ...
                }

                // Apply inverter efficiency 
                double acEnergy = ApplyInverterEfficiency(monthlyEnergy, system.Inverters);

                // Store monthly result
                result.AddMonth(month, new ProductionData
                {
                    GlobHor = meteoData.GlobHor,
                    DiffHor = meteoData.DiffHor,
                    Temperature = meteoData.Temperature,
                    GlobInc = 0, // This would be calculated
                    GlobEff = 0, // This would be calculated
                    EArray = monthlyEnergy, // DC energy
                    EGrid = acEnergy,  // AC energy
                    PerformanceRatio = 0 // This would be calculated
                });
            }

            return result;
        }
    }
}