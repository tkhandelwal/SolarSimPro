// Services/Interfaces/ISimulationService.cs
using System;
using System.Threading.Tasks;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Services.Interfaces
{
    public interface ISimulationService
    {
        Task<SimulationResult> RunSimulationAsync(Guid projectId, Guid solarSystemId);
    }
}