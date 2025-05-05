// Models/ShadingResult.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
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