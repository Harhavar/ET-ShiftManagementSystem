using ET_ShiftManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ET_ShiftManagementSystem.Entities;
using System.Runtime.InteropServices;

namespace Servises.ProjectServises
{
    public interface IProjectServises
    {
        Task<Project> GetProject(int ProjectId);
        long AddProjectAsync(Project project);

        Task<Project> GetProjectAsync(int id);
        public Task<IEnumerable<Project>> GetAllAsync();

        Task<Project> DeleteProjectAsync(int id);

        Task<Project> UpdateAsync(int Id, Project Project);
    }

    public class ProjectServises : IProjectServises
    {
        private readonly ShiftManagementDbContext _shiftDbContext;
        public ProjectServises(ShiftManagementDbContext shiftDbContext)
        {
            _shiftDbContext = shiftDbContext;
        }

        public  long AddProjectAsync(Project project)
        {
            
             _shiftDbContext.projects.Add(project);
           _shiftDbContext.SaveChanges();
            return project.ProjectId;
        }

        public async Task<Project> DeleteProjectAsync(int id)
        {   
            var project = await _shiftDbContext.projects.FirstOrDefaultAsync(r => r.ProjectId == id);

                if (project == null)
                {
                    return null;
                }

                //delete the region
                _shiftDbContext.projects.Remove(project);
                await _shiftDbContext.SaveChangesAsync();


                return project;
            
        }

        public async  Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _shiftDbContext.projects /*.Include(x => x.user)
               Include(x => x.shift).
                Include(x => x.comment)*/
                .ToListAsync();
        }

        public async Task<Project> GetProject(int ProjectId)
        {
            return await _shiftDbContext.projects.AsNoTracking().FirstOrDefaultAsync(a => a.ProjectId == ProjectId);
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            return await _shiftDbContext.projects.FirstOrDefaultAsync(r => r.ProjectId == id);
        }

        public async Task<Project> UpdateAsync(int Id, Project Project)
        {
            var existingProject = await _shiftDbContext.projects.FirstOrDefaultAsync(x => x.ProjectId == Id);

            if (existingProject == null)
            {
                return null;

            }

            existingProject.ProjectName = Project.ProjectName;
            existingProject.Description = Project.Description;
            existingProject.ClientName = Project.ClientName;
            existingProject.ModifieBy = Project.ModifieBy;
            existingProject.ModifieDate = Project.ModifieDate;
            existingProject.CreatedDate = Project.CreatedDate;
            existingProject.IsActive = Project.IsActive;
            existingProject.CreatedBy = Project.CreatedBy;

            await _shiftDbContext.SaveChangesAsync();

            return existingProject;
        }
    }
}