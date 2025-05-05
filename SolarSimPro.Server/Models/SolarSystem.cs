using SolarSimPro.Server.Models;
using System.ComponentModel.DataAnnotations;
using System;

public class SolarSystem
{
    public SolarSystem()
    {
        // Initialize required non-nullable properties
        Name = string.Empty;
        SimulationResults = new List<SimulationResult>();
        Inverters = new List<Inverter>(); // Initialize the Inverters collection
    }

    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public double TotalCapacityKWp { get; set; }
    public int NumberOfModules { get; set; }
    public double ModuleArea { get; set; }

    // Orientation
    public double Tilt { get; set; }
    public double Azimuth { get; set; }

    // References to component databases
    public Guid? PanelModelId { get; set; }
    public Guid? InverterModelId { get; set; }

    // Navigation properties
    public Project Project { get; set; }
    public PanelModel PanelModel { get; set; }
    public InverterModel InverterModel { get; set; }

    // Add this property for inverters
    public List<Inverter> Inverters { get; set; }

    // Simulation results
    public List<SimulationResult> SimulationResults { get; set; }
}