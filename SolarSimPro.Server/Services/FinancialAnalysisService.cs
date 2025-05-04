// Services/FinancialAnalysisService.cs
public class FinancialAnalysisService
{
    public FinancialMetrics CalculateFinancials(SolarSystem system, MonthlyProductionData production, FinancialInputs inputs)
    {
        // Calculate total system cost
        double systemCost = CalculateSystemCost(system, inputs.CostPerWatt);

        // Calculate annual savings
        double annualProduction = production.TotalAnnualProduction;
        double annualSavings = annualProduction * inputs.ElectricityRate;

        // Calculate ROI
        double roi = (annualSavings * inputs.SystemLifetime) / systemCost * 100;

        // Calculate payback period
        double paybackPeriod = systemCost / annualSavings;

        // Calculate NPV (Net Present Value)
        double npv = CalculateNPV(systemCost, annualSavings, inputs.DiscountRate, inputs.SystemLifetime);

        // Calculate LCOE (Levelized Cost of Electricity)
        double lcoe = CalculateLCOE(systemCost, annualProduction, inputs.MaintenanceCost, inputs.DiscountRate, inputs.SystemLifetime);

        return new FinancialMetrics
        {
            TotalSystemCost = systemCost,
            AnnualSavings = annualSavings,
            ROI = roi,
            PaybackPeriod = paybackPeriod,
            NPV = npv,
            LCOE = lcoe
        };
    }
}