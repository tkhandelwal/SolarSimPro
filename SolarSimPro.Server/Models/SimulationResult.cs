using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class SimulationResult
    {
        public Guid Id { get; set; }
        public Guid SolarSystemId { get; set; }
        public DateTime SimulationDate { get; set; } = DateTime.UtcNow;

        // Annual totals
        public double AnnualProduction { get; set; }  // kWh/year
        public double SpecificProduction { get; set; } // kWh/kWp/year
        public double PerformanceRatio { get; set; }  // Percentage

        // Navigation properties
        public SolarSystem SolarSystem { get; set; }
        public List<MonthlyResult> MonthlyResults { get; set; } = new List<MonthlyResult>();
        public LossBreakdown Losses { get; set; }
        public FinancialAnalysis FinancialAnalysis { get; set; }
    }

    public class MonthlyResult
    {
        public Guid Id { get; set; }
        public Guid SimulationResultId { get; set; }
        public int Month { get; set; }
        public double GlobHor { get; set; }  // Global horizontal irradiation (kWh/m²)
        public double DiffHor { get; set; }  // Horizontal diffuse irradiation (kWh/m²)
        public double Temperature { get; set; } // Ambient Temperature (°C)
        public double GlobInc { get; set; }  // Global incident in collector plane (kWh/m²)
        public double GlobEff { get; set; }  // Effective Global, corrected for IAM and shadings (kWh/m²)
        public double EArray { get; set; }   // Effective energy at array output (kWh)
        public double EGrid { get; set; }    // Energy injected into grid (kWh)
        public double PR { get; set; }       // Performance Ratio

        public SimulationResult SimulationResult { get; set; }
    }

    public class LossBreakdown
    {
        public Guid Id { get; set; }
        public Guid SimulationResultId { get; set; }

        public double IAMLoss { get; set; }
        public double SoilingLoss { get; set; }
        public double IrradianceLoss { get; set; }
        public double TemperatureLoss { get; set; }
        public double ModuleQualityLoss { get; set; }
        public double MismatchLoss { get; set; }
        public double OhmicWiringLoss { get; set; }
        public double InverterEfficiencyLoss { get; set; }
        public double ACOhmicLoss { get; set; }
        public double SystemUnavailabilityLoss { get; set; }

        public SimulationResult SimulationResult { get; set; }
    }

    public class FinancialAnalysis
    {
        public Guid Id { get; set; }
        public Guid SimulationResultId { get; set; }

        public double TotalSystemCost { get; set; }
        public double AnnualSavingsYear1 { get; set; }
        public double SimplePaybackPeriod { get; set; }
        public double NPV { get; set; }
        public double ROI { get; set; }
        public double LCOE { get; set; }

        public SimulationResult SimulationResult { get; set; }
    }
}