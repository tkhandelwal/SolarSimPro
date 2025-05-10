// Models/SimulationReport.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class SimulationReport
    {
        public string Title { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double SystemCapacity { get; set; }
        public int NumberOfModules { get; set; }
        public double AnnualProduction { get; set; }
        public double SpecificProduction { get; set; }
        public double PerformanceRatio { get; set; }
        public List<MonthlyResult> MonthlyResults { get; set; } = new List<MonthlyResult>();
        public LossBreakdown Losses { get; set; } = new LossBreakdown();
        public FinancialAnalysis FinancialAnalysis { get; set; } = new FinancialAnalysis();
    }
}