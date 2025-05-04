// ProjectController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ISolarDesignService _designService;

    [HttpGet]
    public async Task<ActionResult<List<Project>>> GetProjects()
    {
        // Return all projects for current user
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(Guid id)
    {
        // Return project details
    }

    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(CreateProjectDto dto)
    {
        // Create new project
    }

    [HttpPost("{id}/simulate")]
    public async Task<ActionResult<SimulationResult>> RunSimulation(Guid id)
    {
        // Execute simulation for project
        var result = await _designService.RunSimulation(id);
        return Ok(result);
    }

    [HttpGet("{id}/report")]
    public async Task<ActionResult> GenerateReport(Guid id)
    {
        // Generate PDF report similar to PVsyst
        var pdfBytes = await _designService.GenerateReport(id);
        return File(pdfBytes, "application/pdf", $"SolarReport_{id}.pdf");
    }
}