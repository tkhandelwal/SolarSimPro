// Services/ProductionCalculationService.cs
public class ProductionCalculationService
{
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