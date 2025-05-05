// Models/ShadingModels.cs
using System;
using System.Collections.Generic;


namespace SolarSimPro.Server.Models
{
    public class ShadingObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ObjectType Type { get; set; }
        public List<Point3D> Points { get; set; } = new List<Point3D>();
        public double Height { get; set; }
    }

    public enum ObjectType
    {
        Building,
        Tree,
        Chimney,
        Other
    }

    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class ShadingResult
    {
        public Dictionary<Guid, List<ShadingTimePoint>> PanelShadingData { get; set; } =
            new Dictionary<Guid, List<ShadingTimePoint>>();

        public void AddShadingData(Guid panelId, int month, int hour, bool isShaded)
        {
            if (!PanelShadingData.ContainsKey(panelId))
            {
                PanelShadingData[panelId] = new List<ShadingTimePoint>();
            }

            PanelShadingData[panelId].Add(new ShadingTimePoint
            {
                Month = month,
                Hour = hour,
                IsShaded = isShaded
            });
        }
    }

    public class ShadingTimePoint
    {
        public int Month { get; set; }
        public int Hour { get; set; }
        public bool IsShaded { get; set; }
    }
}