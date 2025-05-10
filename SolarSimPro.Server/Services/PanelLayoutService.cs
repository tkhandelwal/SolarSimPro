// Services/PanelLayoutService.cs
using SolarSimPro.Server.Models;
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Services
{
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

        private bool IsSuitableForPanels(RoofSection section)
        {
            // Check if roof section is suitable for panels based on orientation and slope
            // For this example, we'll just return true
            return true;
        }

        private List<GridPosition> CalculateOptimalGrid(RoofSection section, PanelSpecifications specs)
        {
            // Calculate optimal positioning of panels on roof section
            var gridPositions = new List<GridPosition>();

            // Simple implementation for example purposes
            double xSpacing = specs.Width + 0.1; // 10cm gap between panels
            double ySpacing = specs.Height + 0.1; // 10cm gap between panels

            // Calculate how many panels can fit in x and y directions
            double sectionWidth = CalculateSectionWidth(section);
            double sectionLength = CalculateSectionLength(section);

            int panelsX = (int)(sectionWidth / xSpacing);
            int panelsY = (int)(sectionLength / ySpacing);

            // Create grid positions
            for (int x = 0; x < panelsX; x++)
            {
                for (int y = 0; y < panelsY; y++)
                {
                    gridPositions.Add(new GridPosition
                    {
                        Position = new Position
                        {
                            X = x * xSpacing,
                            Y = y * ySpacing,
                            Z = 0 // This would be calculated based on roof height
                        }
                    });
                }
            }

            return gridPositions;
        }

        private double CalculateSectionWidth(RoofSection section)
        {
            // Calculate width based on points
            if (section.Points.Count < 2)
                return 0;

            return Math.Abs(section.Points.Max(p => p.X) - section.Points.Min(p => p.X));
        }

        private double CalculateSectionLength(RoofSection section)
        {
            // Calculate length based on points
            if (section.Points.Count < 2)
                return 0;

            return Math.Abs(section.Points.Max(p => p.Y) - section.Points.Min(p => p.Y));
        }
    }

    public class GridPosition
    {
        public Position Position { get; set; } = new Position();
    }
}