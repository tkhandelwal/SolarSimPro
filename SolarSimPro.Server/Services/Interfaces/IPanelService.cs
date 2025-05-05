// Services/Interfaces/IPanelService.cs
using System;
using System.Threading.Tasks;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Services.Interfaces
{
    public interface IPanelService
    {
        Task<PanelModel> GetPanelModelAsync(Guid? panelModelId);
    }
}