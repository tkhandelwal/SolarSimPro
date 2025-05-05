// Models/ShadingObject.cs
using System;
using System.Collections.Generic;

namespace SolarSimPro.Server.Models
{
    public class ShadingObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ObjectType Type { get; set; }
        public List<PointCoordinate> Points { get; set; } = new List<PointCoordinate>();
        public double Height { get; set; }
    }

    public enum ObjectType
    {
        Building,
        Tree,
        Chimney,
        Other
    }
}