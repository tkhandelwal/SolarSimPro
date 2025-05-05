// Services/FinancialAnalysisService.cs
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

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

    // Add the missing method for calculating system cost
    private double CalculateSystemCost(SolarSystem system, double costPerWatt)
    {
        // Calculate total system cost based on capacity and cost per watt
        return system.TotalCapacityKWp * 1000 * costPerWatt;
    }

    // Method to calculate Net Present Value
    private double CalculateNPV(double initialInvestment, double annualSavings, double discountRate, int years)
    {
        double npv = -initialInvestment;

        for (int year = 1; year <= years; year++)
        {
            npv += annualSavings / Math.Pow(1 + discountRate, year);
        }

        return npv;
    }

    // Method to calculate Levelized Cost of Electricity (LCOE)
    private double CalculateLCOE(double systemCost, double annualProduction, double annualMaintenance, double discountRate, int years)
    {
        double totalCost = systemCost;
        double totalProduction = 0;
        double annualDegradation = 0.005; // 0.5% per year typical panel degradation

        for (int year = 1; year <= years; year++)
        {
            // Production decreases each year due to panel degradation
            double yearlyProduction = annualProduction * Math.Pow(1 - annualDegradation, year - 1);
            totalProduction += yearlyProduction / Math.Pow(1 + discountRate, year);

            // Add maintenance costs
            totalCost += annualMaintenance / Math.Pow(1 + discountRate, year);
        }

        return totalCost / totalProduction;
    }
}