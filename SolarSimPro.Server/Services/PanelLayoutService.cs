// Services/PanelLayoutService.cs
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

public class PanelLayoutService
{
    public List<Panel> GenerateOptimalLayout(RoofGeometry roof, PanelSpecifications panelSpec)
    {
        List<Panel> panels = new List<Panel>();

        // Algorithm steps:
        // 1. Identify viable roof sections (orientation, shade-free)
        // 2. Define row spacing based on latitude (shadow analysis)
        // 3. Start from most southern edge of roof (northern hemisphere)
        // 4. Place panels in rows with appropriate spacing

        // For each roof section
        foreach (var section in roof.Sections)
        {
            if (IsSuitableForPanels(section))
            {
                var layoutGrid = CalculateOptimalGrid(section, panelSpec);

                foreach (var gridPosition in layoutGrid)
                {
                    panels.Add(new Panel
                    {
                        Id = Guid.NewGuid(),
                        Position = gridPosition.Position,
                        Orientation = new Orientation
                        {
                            Tilt = section.Slope,
                            Azimuth = section.Azimuth
                        },
                        PanelSpecification = panelSpec
                    });
                }
            }
        }

        return panels;
    }
}