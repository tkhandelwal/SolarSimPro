// Services/ReportService.cs
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace SolarSimPro.Server.Services
{
    public class ReportService : IReportService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISimulationService _simulationService;

        public ReportService(
            IProjectRepository projectRepository,
            ISimulationService simulationService)
        {
            _projectRepository = projectRepository;
            _simulationService = simulationService;
        }

        public SimulationReport GenerateReport(Guid projectId)
        {
            // Implementation to generate a report model
            throw new NotImplementedException();
        }

        public async Task<byte[]> GeneratePdfReportAsync(Guid projectId, Guid solarSystemId)
        {
            // Get project and simulation data
            var project = await _projectRepository.GetProjectWithDetailsAsync(projectId);
            var solarSystem = project.Systems.FirstOrDefault(s => s.Id == solarSystemId);

            if (solarSystem == null)
                throw new KeyNotFoundException("Solar system not found");

            var simulation = await _simulationService.RunSimulationAsync(projectId, solarSystemId);

            // Create PDF document
            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12);
            var titleFont = new XFont("Arial", 18, XFontStyleEx.Bold);
            var headerFont = new XFont("Arial", 14, XFontStyleEx.Bold);

            // Page dimensions
            var pageWidth = page.Width;
            var pageHeight = page.Height;

            // Title section
            gfx.DrawString("Solar System Simulation Report", titleFont, XBrushes.Black,
                new XRect(0, 20, pageWidth.Point, 30), XStringFormats.Center);

            gfx.DrawString($"Project: {project.Name}", headerFont, XBrushes.Black,
                new XRect(50, 60, pageWidth.Point, 20), XStringFormats.Default);

            // Project details
            int y = 100;
            gfx.DrawString("Project Information", headerFont, XBrushes.Black, new XPoint(50, y));
            y += 20;

            gfx.DrawString($"Location: {project.Location}", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Latitude: {project.Latitude:N2}°N", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Longitude: {project.Longitude:N2}°E", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Altitude: {project.Altitude} m", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Time Zone: {project.TimeZone}", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Albedo: {project.Albedo}", font, XBrushes.Black, new XPoint(50, y)); y += 30;

            // System details
            gfx.DrawString("System Information", headerFont, XBrushes.Black, new XPoint(50, y)); y += 20;

            gfx.DrawString($"Number of modules: {solarSystem.NumberOfModules} units", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"System power: {solarSystem.TotalCapacityKWp:N2} kWp", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Module area: {solarSystem.ModuleArea:N1} m²", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Orientation - Tilt/Azimuth: {solarSystem.Tilt:N1} / {solarSystem.Azimuth:N1}°", font, XBrushes.Black, new XPoint(50, y)); y += 30;

            // Results summary
            gfx.DrawString("Results Summary", headerFont, XBrushes.Black, new XPoint(50, y)); y += 20;

            gfx.DrawString($"Produced Energy: {simulation.AnnualProduction:N0} kWh/year", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Specific production: {simulation.SpecificProduction:N0} kWh/kWp/year", font, XBrushes.Black, new XPoint(50, y)); y += 15;
            gfx.DrawString($"Performance Ratio: {simulation.PerformanceRatio:P2}", font, XBrushes.Black, new XPoint(50, y)); y += 30;

            // Add monthly results table
            DrawMonthlyResultsTable(gfx, simulation.MonthlyResults, new XPoint(50, y));
            y += 200; // Approximate height of the table

            // Add loss diagram
            DrawLossDiagram(gfx, simulation.Losses, new XPoint(50, y));
            y += 300; // Height of loss diagram

            // Add financial analysis
            DrawFinancialAnalysis(gfx, simulation.FinancialAnalysis, pageWidth.Point, 30, new XPoint(50, y));

            // Add charts on new page
            var page2 = document.AddPage();
            var gfx2 = XGraphics.FromPdfPage(page2);
            gfx2.DrawString("Production Charts", titleFont, XBrushes.Black,
                new XRect(0, 20, pageWidth.Point, 30), XStringFormats.Center);

            DrawMonthlyProductionChart(gfx2, simulation.MonthlyResults, new XPoint(50, 60), 500, 300);
            DrawPerformanceRatioChart(gfx2, simulation.MonthlyResults, new XPoint(50, 400), 500, 300);

            // Save the PDF to memory stream
            var stream = new MemoryStream();
            document.Save(stream);

            return stream.ToArray();
        }

        private void DrawMonthlyResultsTable(XGraphics gfx, List<MonthlyResult> results, XPoint position)
        {
            // Implementation of drawing the monthly results table
        }

        private void DrawLossDiagram(XGraphics gfx, LossBreakdown losses, XPoint position)
        {
            // Implementation of drawing the loss diagram
        }

        private void DrawFinancialAnalysis(XGraphics gfx, FinancialAnalysis financial, double width, int v, XPoint position)
        {
            // Implementation of drawing the financial analysis section
        }

        private void DrawMonthlyProductionChart(XGraphics gfx, List<MonthlyResult> results, XPoint position, double width, double height)
        {
            // Implementation of drawing the monthly production chart
        }

        private void DrawPerformanceRatioChart(XGraphics gfx, List<MonthlyResult> results, XPoint position, double width, double height)
        {
            // Implementation of drawing the performance ratio chart
        }
    }
}