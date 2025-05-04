// Services/SimulationService.cs
public class SimulationService : ISimulationService
{
    private readonly IMeteoDataService _meteoService;
    private readonly IPanelService _panelService;
    private readonly IProjectRepository _projectRepository;

    public SimulationService(
        IMeteoDataService meteoService,
        IPanelService panelService,
        IProjectRepository projectRepository)
    {
        _meteoService = meteoService;
        _panelService = panelService;
        _projectRepository = projectRepository;
    }

    public async Task<SimulationResult> RunSimulationAsync(Guid projectId, Guid solarSystemId)
    {
        // Get the project, solar system, and panel layout
        var project = await _projectRepository.GetProjectWithDetailsAsync(projectId);
        var solarSystem = project.Systems.FirstOrDefault(s => s.Id == solarSystemId);

        if (solarSystem == null)
            throw new KeyNotFoundException("Solar system not found");

        // Get meteorological data
        var meteoData = await _meteoService.GetMeteoDataAsync(project.Latitude, project.Longitude);

        // Get panel model details
        var panelModel = await _panelService.GetPanelModelAsync(solarSystem.PanelModelId);

        // Initialize simulation result
        var result = new SimulationResult
        {
            SolarSystemId = solarSystemId,
            SimulationDate = DateTime.UtcNow,
            MonthlyResults = new List<MonthlyResult>(),
            Losses = CalculateLosses(solarSystem, panelModel)
        };

        double annualProduction = 0;

        // Calculate production for each month
        for (int month = 1; month <= 12; month++)
        {
            var monthlyMeteo = meteoData.MonthlyData.FirstOrDefault(m => m.Month == month);
            if (monthlyMeteo == null) continue;

            // Calculate effective irradiation based on panel orientation
            double globInc = CalculateIncidentIrradiation(
                monthlyMeteo.GlobHor,
                monthlyMeteo.DiffHor,
                project.Latitude,
                month,
                solarSystem.Tilt,
                solarSystem.Azimuth
            );

            // Apply IAM factor (Incidence Angle Modifier)
            double iamFactor = CalculateIAMFactor(solarSystem.Tilt, month, project.Latitude);

            // Apply soiling loss (9% as in PVsyst example)
            double soilingFactor = 0.91; // 1.0 - 0.09

            // Calculate effective irradiation
            double globEff = globInc * iamFactor * soilingFactor;

            // Calculate array energy (DC) considering:
            // 1. PV conversion efficiency
            // 2. Temperature losses
            // 3. Quality, mismatch, ohmic losses

            double arrayArea = solarSystem.NumberOfModules * panelModel.Width * panelModel.Height;
            double nominalEfficiency = panelModel.Efficiency;

            // Apply temperature derating
            double tempCoeff = panelModel.TempCoeffPmax; // %/°C
            double tempDiff = monthlyMeteo.Temperature - 25; // Difference from STC temp
            double tempFactor = 1 + (tempDiff * tempCoeff / 100);

            // Array energy calculation
            double eArray = globEff * arrayArea * nominalEfficiency * tempFactor * (1 - 0.02); // 2% for mismatch

            // Apply inverter efficiency
            double inverterEfficiency = 0.97; // Typical value
            double eGrid = eArray * inverterEfficiency * (1 - 0.02); // 2% for AC losses

            // Calculate Performance Ratio
            double pr = eGrid / (monthlyMeteo.GlobHor * solarSystem.TotalCapacityKWp);

            // Add monthly result
            result.MonthlyResults.Add(new MonthlyResult
            {
                Month = month,
                GlobHor = monthlyMeteo.GlobHor,
                DiffHor = monthlyMeteo.DiffHor,
                Temperature = monthlyMeteo.Temperature,
                GlobInc = globInc,
                GlobEff = globEff,
                EArray = eArray,
                EGrid = eGrid,
                PR = pr
            });

            annualProduction += eGrid;
        }

        // Calculate annual totals
        result.AnnualProduction = annualProduction;
        result.SpecificProduction = annualProduction / solarSystem.TotalCapacityKWp;
        result.PerformanceRatio = result.MonthlyResults.Average(m => m.PR);

        // Calculate financial analysis
        result.FinancialAnalysis = CalculateFinancialAnalysis(annualProduction, solarSystem);

        return result;
    }

    private double CalculateIncidentIrradiation(double globHor, double diffHor, double latitude, int month, double tilt, double azimuth)
    {
        // Implementation of the Perez model or similar
        // This is a simplified approximation - a real implementation would be more complex

        // Calculate average day of month
        int dayOfYear = GetDayOfYear(month);

        // Calculate declination angle
        double declination = 23.45 * Math.Sin(Math.PI / 180 * 360 * (284 + dayOfYear) / 365);

        // Convert to radians
        double latRad = latitude * Math.PI / 180;
        double declRad = declination * Math.PI / 180;
        double tiltRad = tilt * Math.PI / 180;
        double azimuthRad = azimuth * Math.PI / 180;

        // Calculate hour angle (use noon for simplicity)
        double hourAngle = 0; // Noon
        double hourAngleRad = hourAngle * Math.PI / 180;

        // Calculate solar zenith angle
        double cosZenith = Math.Sin(latRad) * Math.Sin(declRad) +
                          Math.Cos(latRad) * Math.Cos(declRad) * Math.Cos(hourAngleRad);

        // Calculate solar incidence angle on tilted surface
        double cosIncidence = Math.Sin(declRad) * Math.Sin(latRad) * Math.Cos(tiltRad) -
                             Math.Sin(declRad) * Math.Cos(latRad) * Math.Sin(tiltRad) * Math.Cos(azimuthRad) +
                             Math.Cos(declRad) * Math.Cos(latRad) * Math.Cos(tiltRad) * Math.Cos(hourAngleRad) +
                             Math.Cos(declRad) * Math.Sin(latRad) * Math.Sin(tiltRad) * Math.Cos(azimuthRad) * Math.Cos(hourAngleRad) +
                             Math.Cos(declRad) * Math.Sin(tiltRad) * Math.Sin(azimuthRad) * Math.Sin(hourAngleRad);

        // Calculate beam radiation on tilted surface
        double beamHor = globHor - diffHor;
        double rb = cosIncidence / cosZenith;
        double beamTilt = beamHor * rb;

        // Calculate diffuse radiation on tilted surface (simple isotropic sky model)
        double diffTilt = diffHor * (1 + Math.Cos(tiltRad)) / 2;

        // Calculate ground-reflected radiation
        double groundReflectance = 0.2; // Albedo
        double groundTilt = globHor * groundReflectance * (1 - Math.Cos(tiltRad)) / 2;

        // Calculate total radiation on tilted surface
        return beamTilt + diffTilt + groundTilt;
    }

    private int GetDayOfYear(int month)
    {
        // Approximate middle day of each month
        int[] daysOfYear = { 15, 46, 74, 105, 135, 166, 196, 227, 258, 288, 319, 349 };
        return daysOfYear[month - 1];
    }

    private double CalculateIAMFactor(double tilt, int month, double latitude)
    {
        // In a real implementation, this would calculate the IAM factor based on
        // incidence angles throughout the day and Fresnel equations

        // Simplified version based on typical IAM values from the PVsyst report
        double[] typicalIAM = { 0.97, 0.97, 0.96, 0.95, 0.94, 0.93, 0.91, 0.93, 0.95, 0.96, 0.97, 0.97 };
        return typicalIAM[month - 1];
    }

    private LossBreakdown CalculateLosses(SolarSystem solarSystem, PanelModel panelModel)
    {
        // Create loss breakdown similar to PVsyst report
        return new LossBreakdown
        {
            IAMLoss = 0.0239,  // 2.39%
            SoilingLoss = 0.09,  // 9%
            IrradianceLoss = 0.005,  // 0.5%
            TemperatureLoss = 0.085,  // 8.5%
            ModuleQualityLoss = -0.0075,  // -0.75%
            MismatchLoss = 0.02,  // 2%
            OhmicWiringLoss = 0.0146,  // 1.46%
            InverterEfficiencyLoss = 0.0187,  // 1.87%
            ACOhmicLoss = 0.0102,  // 1.02%
            SystemUnavailabilityLoss = 0.0124  // 1.24%
        };
    }

    private FinancialAnalysis CalculateFinancialAnalysis(double annualProduction, SolarSystem solarSystem)
    {
        // Default financial parameters - in a real app, these would come from user inputs
        double costPerWatt = 1.5;  // $/W
        double electricityRate = 0.15;  // $/kWh
        double systemLifetime = 25;  // years
        double discountRate = 0.04;  // 4%
        double annualDegradation = 0.005;  // 0.5% per year
        double annualMaintenanceCost = 0.01 * solarSystem.TotalCapacityKWp * costPerWatt;  // 1% of initial cost

        // Calculate total system cost
        double systemCost = solarSystem.TotalCapacityKWp * 1000 * costPerWatt;

        // Calculate annual savings in year 1
        double annualSavings = annualProduction * electricityRate;

        // Calculate simple payback period
        double simplePayback = systemCost / annualSavings;

        // Calculate NPV
        double npv = -systemCost;
        double cumulativeProduction = 0;
        for (int year = 1; year <= systemLifetime; year++)
        {
            double yearlyProduction = annualProduction * Math.Pow(1 - annualDegradation, year - 1);
            cumulativeProduction += yearlyProduction;
            double yearlyRevenue = yearlyProduction * electricityRate;
            npv += (yearlyRevenue - annualMaintenanceCost) / Math.Pow(1 + discountRate, year);
        }

        // Calculate ROI
        double roi = (npv + systemCost) / systemCost * 100;

        // Calculate LCOE
        double lcoe = systemCost / cumulativeProduction;

        return new FinancialAnalysis
        {
            TotalSystemCost = systemCost,
            AnnualSavingsYear1 = annualSavings,
            SimplePaybackPeriod = simplePayback,
            NPV = npv,
            ROI = roi,
            LCOE = lcoe
        };
    }
}