// Models/RoofGeometry.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class RoofGeometry
    {
        public Guid Id { get; set; }
        public double Width { get; set; }  // In meters
        public double Length { get; set; }  // In meters
        public List<RoofSection> Sections { get; set; } = new List<RoofSection>();
    }

    public class RoofSection
    {
        public int Id { get; set; }
        public List<PointCoordinate> Points { get; set; } = new List<PointCoordinate>();
        public double Slope { get; set; }  // In degrees
        public double Azimuth { get; set; }  // In degrees
    }

    public class PointCoordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}