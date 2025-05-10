// Controllers/SolarSystemsController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarSimPro.Server.Data;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/systems")]
    public class SolarSystemsController : ControllerBase
    {
        private readonly SolarDesignDbContext _context;

        public SolarSystemsController(SolarDesignDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolarSystem>>> GetSolarSystems(Guid projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                return NotFound("Project not found");

            var systems = await _context.SolarSystems
                .Where(s => s.ProjectId == projectId)
                .ToListAsync();

            return Ok(systems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SolarSystem>> GetSolarSystem(Guid projectId, Guid id)
        {
            var system = await _context.SolarSystems
                .FirstOrDefaultAsync(s => s.ProjectId == projectId && s.Id == id);

            if (system == null)
                return NotFound("Solar system not found");

            return Ok(system);
        }

        // Ensure this works in SolarSystemsController.cs
        [HttpPost]
        public async Task<ActionResult<SolarSystem>> CreateSolarSystem(Guid projectId, SolarSystem system)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                return NotFound("Project not found");

            system.Id = Guid.NewGuid();
            system.ProjectId = projectId;

            _context.SolarSystems.Add(system);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSolarSystem),
                new { projectId = projectId, id = system.Id }, system);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSolarSystem(Guid projectId, Guid id, SolarSystem system)
        {
            if (id != system.Id || projectId != system.ProjectId)
                return BadRequest();

            _context.Entry(system).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolarSystemExists(id))
                    return NotFound("Solar system not found");
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolarSystem(Guid projectId, Guid id)
        {
            var system = await _context.SolarSystems
                .FirstOrDefaultAsync(s => s.ProjectId == projectId && s.Id == id);

            if (system == null)
                return NotFound("Solar system not found");

            _context.SolarSystems.Remove(system);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolarSystemExists(Guid id)
        {
            return _context.SolarSystems.Any(e => e.Id == id);
        }
    }
}