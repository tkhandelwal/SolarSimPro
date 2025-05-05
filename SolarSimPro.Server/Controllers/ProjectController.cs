// Controllers/ProjectsController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;

namespace SolarSimPro.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _repository;
        private readonly ISimulationService _simulationService;
        private readonly IReportService _reportService;

        public ProjectsController(
            IProjectRepository repository,
            ISimulationService simulationService,
            IReportService reportService)
        {
            _repository = repository;
            _simulationService = simulationService;
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _repository.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {
            try
            {
                var project = await _repository.GetProjectByIdAsync(id);
                return Ok(project);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            var newProject = await _repository.CreateProjectAsync(project);
            return CreatedAtAction(nameof(GetProject), new { id = newProject.Id }, newProject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, Project project)
        {
            if (id != project.Id)
                return BadRequest();

            try
            {
                await _repository.UpdateProjectAsync(project);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _repository.DeleteProjectAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/simulate/{systemId}")]
        public async Task<ActionResult<SimulationResult>> RunSimulation(Guid id, Guid systemId)
        {
            try
            {
                var result = await _simulationService.RunSimulationAsync(id, systemId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/report/{systemId}")]
        public async Task<ActionResult> GenerateReport(Guid id, Guid systemId)
        {
            try
            {
                var pdfBytes = await _reportService.GeneratePdfReportAsync(id, systemId);
                return File(pdfBytes, "application/pdf", $"SolarReport_{id}_{systemId}.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}