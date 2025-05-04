// Controllers/ReportController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    [HttpGet("{projectId}")]
    public ActionResult<SimulationReport> GenerateReport(Guid projectId)
    {
        // Generate report similar to the PVsyst PDF you shared
        return _reportService.GenerateReport(projectId);
    }
}