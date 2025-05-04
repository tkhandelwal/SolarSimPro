// Models/SimulationResult.cs
public class SimulationResult
{
    public Guid Id { get; set; }
    public Guid SolarSystemId { get; set; }
    public DateTime SimulationDate { get; set; } = DateTime.UtcNow;

    // Annual totals
    public double AnnualProduction { get; set; }  // kWh/year
    public double SpecificProduction { get; set; } // kWh/kWp/year
    public double PerformanceRatio { get; set; }  // Percentage

    // Monthly breakdown
    public List<MonthlyResult> MonthlyResults { get; set; } = new List<MonthlyResult>();

    // Loss details for diagram
    public LossBreakdown Losses { get; set; }

    // Financial metrics
    public FinancialAnalysis FinancialAnalysis { get; set; }
}

public class MonthlyResult
{
    public int Month { get; set; }
    public double GlobHor { get; set; }  // Global horizontal irradiation (kWh/m²)
    public double DiffHor { get; set; }  // Horizontal diffuse irradiation (kWh/m²)
    public double Temperature { get; set; } // Ambient Temperature (°C)
    public double GlobInc { get; set; }  // Global incident in collector plane (kWh/m²)
    public double GlobEff { get; set; }  // Effective Global, corrected for IAM and shadings (kWh/m²)
    public double EArray { get; set; }   // Effective energy at array output (kWh)
    public double EGrid { get; set; }    // Energy injected into grid (kWh)
    public double PR { get; set; }       // Performance Ratio
}