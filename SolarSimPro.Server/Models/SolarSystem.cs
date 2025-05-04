// Models/SolarSystem.cs
using Microsoft.AspNetCore.Mvc.RazorPages;

public class SolarSystem
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public double TotalCapacityKWp { get; set; }
    public int NumberOfModules { get; set; }
    public double ModuleArea { get; set; }

    // Orientation
    public double Tilt { get; set; }
    public double Azimuth { get; set; }

    // References to component databases
    public Guid PanelModelId { get; set; }
    public Guid InverterModelId { get; set; }

    // Navigation properties
    public PanelModel PanelModel { get; set; }
    public InverterModel InverterModel { get; set; }
    public List<PanelArray> Arrays { get; set; } = new List<PanelArray>();

    // Simulation results
    public SimulationResult SimulationResult { get; set; }
}