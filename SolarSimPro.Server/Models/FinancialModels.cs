// Models/FinancialModels.cs
using System;

namespace SolarSimPro.Server.Models
{
    public class FinancialMetrics
    {
        public double TotalSystemCost { get; set; }
        public double AnnualSavings { get; set; }
        public double ROI { get; set; }
        public double PaybackPeriod { get; set; }
        public double NPV { get; set; }
        public double LCOE { get; set; }
    }

    public class FinancialInputs
    {
        public double CostPerWatt { get; set; } = 1.5; // Default $1.50/W
        public double ElectricityRate { get; set; } = 0.15; // Default $0.15/kWh
        public int SystemLifetime { get; set; } = 25; // Default 25 years
        public double DiscountRate { get; set; } = 0.04; // Default 4%
        public double MaintenanceCost { get; set; } = 0.01; // Default 1% of system cost per year
        public double AnnualDegradation { get; set; } = 0.005; // Default 0.5% per year
    }
}