// Services/Interfaces/ILocationService.cs
using System.Threading.Tasks;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Services.Interfaces
{
    public interface ILocationService
    {
        Task<GeoLocation> GetLocationDetailsAsync(string address);
        Task<string> GetTimeZoneAsync(double latitude, double longitude);
        Task<MeteoData> GetMeteoDataAsync(double latitude, double longitude);
    }
}