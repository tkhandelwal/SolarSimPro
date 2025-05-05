// Services/PanelService.cs
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolarSimPro.Server.Data;
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace SolarSimPro.Server.Services
{
    public class PanelService : IPanelService
    {
        private readonly SolarDesignDbContext _context;

        public PanelService(SolarDesignDbContext context)
        {
            _context = context;
        }

        public async Task<PanelModel> GetPanelModelAsync(Guid? panelModelId)
        {
            if (panelModelId == null)
            {
                // Return a default panel model if not specified
                return new PanelModel
                {
                    Id = Guid.NewGuid(),
                    Manufacturer = "Jinkosolar",
                    ModelName = "JKM-580N-72HL4-BDV",
                    NominalPowerWp = 580,
                    Efficiency = 0.2245,
                    Width = 1.134,
                    Height = 2.278,
                    Thickness = 0.035,
                    Weight = 35.1,
                    VocStc = 49.55,
                    IscStc = 14.79,
                    VmppStc = 41.30,
                    ImppStc = 14.05,
                    TempCoeffPmax = -0.34,
                    TempCoeffVoc = -0.28,
                    TempCoeffIsc = 0.048
                };
            }

            var panelModel = await _context.PanelModels.FindAsync(panelModelId);

            if (panelModel == null)
                throw new KeyNotFoundException($"Panel model with ID {panelModelId} not found");

            return panelModel;
        }
    }
}