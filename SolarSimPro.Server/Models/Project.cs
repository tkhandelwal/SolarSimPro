using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SolarSimPro.Server.Models
{
    public class Project
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string ClientName { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        [StringLength(50)]
        public string TimeZone { get; set; }

        public double Albedo { get; set; } = 0.20; // Default like in PVsyst

        public ProjectType Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public List<SolarSystem> Systems { get; set; } = new List<SolarSystem>();
    }

    public enum ProjectType
    {
        Residential,
        Commercial,
        Industrial,
        Utility
    }
}