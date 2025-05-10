using System;
using System.ComponentModel.DataAnnotations;

namespace SolarSimPro.Server.Models
{
    public class InverterModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Manufacturer { get; set; }

        [Required]
        [StringLength(100)]
        public string? ModelName { get; set; }

        public double NominalPowerAc { get; set; } // kW
        public double MaxEfficiency { get; set; } // %

        // Operating voltage range
        public double MinInputVoltage { get; set; } // V
        public double MaxInputVoltage { get; set; } // V

        // Max input current
        public double MaxInputCurrent { get; set; } // A

        // Number of MPPT inputs
        public int NumberOfMpptInputs { get; set; }
    }
}