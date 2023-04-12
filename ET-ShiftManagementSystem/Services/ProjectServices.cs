using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ET_ShiftManagementSystem.Services
{
    public interface IProjectServices
    {
        public Task<Projects> AddProject(Guid tenantId, Projects project);
        public Task<Projects> DeleteProject(Guid projectId);
        public Task<Projects> EditProject(Guid ProjectId, Projects project/*, List<UserShift> userShifts*/);
        public Projects GetProjectById(Guid projectId);
        public List<Projects> GetProjectsData();
        public List<Projects> GetProjectsData(Guid tenentId);
        public int GetProjectsCount(Guid tenentId);
        object UserShift(Guid projectId);
        //  UserShift(Guid projectId);
    }
    public class ProjectServices : IProjectServices
    {
        private readonly ShiftManagementDbContext _dbContext;

        public ProjectServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Projects> AddProject(Guid tenantId, Projects project)
        {
            project.ProjectId = Guid.NewGuid();
            project.TenentId = tenantId;
            project.CreatedDate = DateTime.Now;
            project.LastModifiedDate = DateTime.Now;
            project.Status = "active";
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
            return project;

        }

        public async Task<Projects> DeleteProject(Guid projectId)
        {
            var Remove = await _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == projectId);

            if (Remove == null)
            {
                return null;
            }

            _dbContext.Projects.Remove(Remove);
            await _dbContext.SaveChangesAsync();
            return Remove;
        }

        public async Task<Projects> EditProject(Guid ProjectId, Projects project/*, List<UserShift> userShifts*/)
        {
            var existingProject = await _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == ProjectId);

            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.LastModifiedDate = DateTime.Now;
                existingProject.Status = project.Status;
                await _dbContext.SaveChangesAsync();

                return existingProject;

            }

            return null;

        }

        public Projects GetProjectById(Guid projectId)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.ProjectId == projectId);

            if (project == null)
            {
                return null;
            }
            return project;
        }

        public int GetProjectsCount(Guid tenentId)
        {
            return _dbContext.Projects.Where(x => x.TenentId == tenentId).Count();
        }

        public List<Projects> GetProjectsData()
        {
            return _dbContext.Projects.ToList();
        }

        public List<Projects> GetProjectsData(Guid tenentId)
        {
            var responce = _dbContext.Projects.Where(x => x.TenentId == tenentId).ToList();
            if (responce == null)
            {
                return null;

            }
            return responce;
        }

        public object UserShift(Guid projectId)
        {
            throw new NotImplementedException();
        }
    }
}
