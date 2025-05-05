// Services/ProductionCalculationService.cs
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

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

                // For each panel in the system
                foreach (var array in system.Arrays)
                {
                    foreach (var panel in array.Panels)
                    {
                        // Calculate effective irradiation considering:
                        // 1. Panel orientation (tilt/azimuth)
                        // 2. Shading effects from nearby objects
                        // 3. IAM (Incidence Angle Modifier) factor
                        // 4. Soiling losses

                        double effectiveIrradiation = CalculateEffectiveIrradiation(
                            dailyIrradiation,
                            panel,
                            shading,
                            month,
                            day
                        );

                        // Calculate DC energy produced by panel
                        double dcEnergy = effectiveIrradiation * panel.Area * panel.Efficiency;

                        // Apply temperature derating
                        dcEnergy = ApplyTemperatureDerating(dcEnergy, meteoData.Temperature, panel);

                        // Apply other losses (mismatch, wiring, etc.)
                        dcEnergy = ApplySystemLosses(dcEnergy, system.LossFactors);

                        // Add to daily and monthly totals
                        monthlyEnergy += dcEnergy;
                    }
                }
            }

            // Apply inverter efficiency 
            double acEnergy = ApplyInverterEfficiency(monthlyEnergy, system.Inverters);

            // Store monthly result
            result.AddMonth(month, new ProductionData
            {
                GlobHor = meteoData.GlobalHorizontalIrradiation,
                DiffHor = meteoData.DiffuseHorizontalIrradiation,
                Temperature = meteoData.AverageTemperature,
                GlobInc = meteoData.IncidentIrradiation,
                GlobEff = meteoData.EffectiveIrradiation,
                EArray = monthlyEnergy, // DC energy
                EGrid = acEnergy,  // AC energy
                PerformanceRatio = acEnergy / (meteoData.IncidentIrradiation * system.TotalCapacityKWp)
            });
        }

        return result;
    }
}