// Services/ProjectRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolarSimPro.Server.Data;
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace SolarSimPro.Server.Services
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly SolarDesignDbContext _context;

        public ProjectRepository(SolarDesignDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(Guid id)
        {
            return await _context.Projects.FindAsync(id) ??
                throw new KeyNotFoundException($"Project with ID {id} not found");
        }

        public async Task<Project> GetProjectWithDetailsAsync(Guid id)
        {
            var project = await _context.Projects
                .Include(p => p.Systems)
                    .ThenInclude(s => s.PanelModel)
                .Include(p => p.Systems)
                    .ThenInclude(s => s.InverterModel)
                .Include(p => p.Systems)
                    .ThenInclude(s => s.SimulationResults)
                        .ThenInclude(sr => sr.MonthlyResults)
                .Include(p => p.Systems)
                    .ThenInclude(s => s.SimulationResults)
                        .ThenInclude(sr => sr.Losses)
                .Include(p => p.Systems)
                    .ThenInclude(s => s.SimulationResults)
                        .ThenInclude(sr => sr.FinancialAnalysis)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {id} not found");

            return project;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            project.Id = Guid.NewGuid();
            project.CreatedAt = DateTime.UtcNow;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            project.UpdatedAt = DateTime.UtcNow;

            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}