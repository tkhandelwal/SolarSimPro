// Models/Panel.cs
using System;

namespace SolarSimPro.Server.Models
{
    public class Panel
    {
        public Guid Id { get; set; }
        public Position Position { get; set; } = new Position();
        public Orientation Orientation { get; set; } = new Orientation();
        public PanelSpecifications PanelSpecification { get; set; } = new PanelSpecifications();
        public double Area => PanelSpecification.Width * PanelSpecification.Height;
        public double Efficiency => PanelSpecification.Efficiency;
    }

    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class Orientation
    {
        public double Tilt { get; set; }  // In degrees from horizontal
        public double Azimuth { get; set; }  // In degrees (0 = North, 90 = East, etc.)
    }

    public class PanelSpecifications
    {
        public double Width { get; set; }  // In meters
        public double Height { get; set; }  // In meters
        public double Thickness { get; set; }  // In meters
        public double Efficiency { get; set; }  // As a fraction (e.g., 0.21 for 21%)
        public double NominalPower { get; set; }  // In watts
    }
}