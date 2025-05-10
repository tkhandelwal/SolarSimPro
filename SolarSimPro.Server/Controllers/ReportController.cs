// Controllers/ReportController.cs
using System;
using Microsoft.AspNetCore.Mvc;
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;

namespace SolarSimPro.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{projectId}")]
        public ActionResult<SimulationReport> GenerateReport(Guid projectId)
        {
            // Generate report similar to the PVsyst PDF you shared
            return _reportService.GenerateReport(projectId);
        }
    }
}