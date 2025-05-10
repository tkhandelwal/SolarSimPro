// Models/Inverter.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace SolarSimPro.Server.Models
{
    public class Inverter
    {
        public Guid Id { get; set; }
        public Guid SolarSystemId { get; set; }

        [Required]
        public Guid InverterModelId { get; set; }

        public int StringsPerMppt { get; set; }
        public int ModulesPerString { get; set; }

        // Make the navigation property nullable
        public SolarSystem? SolarSystem { get; set; }
        public InverterModel? InverterModel { get; set; }
    }
}