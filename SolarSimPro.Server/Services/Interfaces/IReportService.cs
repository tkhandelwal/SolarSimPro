// Services/Interfaces/IReportService.cs
using System;
using System.Threading.Tasks;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Services.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GeneratePdfReportAsync(Guid projectId, Guid solarSystemId);
        SimulationReport GenerateReport(Guid projectId);
    }
}