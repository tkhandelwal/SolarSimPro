// Services/Interfaces/IMeteoDataService.cs
using System.Threading.Tasks;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Services.Interfaces
{
    public interface IMeteoDataService
    {
        Task<MeteoData> GetMeteoDataAsync(double latitude, double longitude);
    }
}