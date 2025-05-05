// Services/Interfaces/IProjectRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Services.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(Guid id);
        Task<Project> GetProjectWithDetailsAsync(Guid id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(Guid id);
    }
}