using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ET_ShiftManagementSystem.Services
{
    public interface IProjectServices
    {
        Task<Projects> AddProject(Guid tenantId, Projects project);
        Task<Projects> DeleteProject(Guid projectId);
        Task<Projects> EditProject(Guid ProjectId, Projects project /*, List<UserShift>  userShifts*/);
        Task<Projects> GetProjectById(Guid projectId);
        public Task<IEnumerable<Projects>> GetProjectsData();
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
            project.ProjectId=Guid.NewGuid();
            project.TenentId = tenantId;
            project.CreatedDate= DateTime.Now;
            project.LastModifiedDate= DateTime.Now;
            project.Status = "active";
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
            return project;

        }

        public async Task<Projects> DeleteProject(Guid projectId)
        {
            var Remove = await _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId==projectId);

            if (Remove == null)
            {
                return null;
            }

            _dbContext.Projects.Remove(Remove);
            await  _dbContext.SaveChangesAsync();
            return Remove;
        }

        public async Task<Projects> EditProject(Guid ProjectId, Projects project /*, List<UserShift> userShifts*/)
        {
            var existingProject =await  _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == ProjectId);

            if( existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.LastModifiedDate = DateTime.Now;
                existingProject.Status = project.Status;
                await _dbContext.SaveChangesAsync();
                return existingProject;

                //var existingUserShift = _dbContext.UserShifts.Where(x => x.ProjectId == ProjectId).ToList();

                //if (existingUserShift.Any())
                //{
                //    foreach (var item in existingUserShift)
                //    {
                //        var a = existingUserShift.Where(x=> x.ProjectId == ProjectId).Select(x => x.ShiftId).FirstOrDefault();
                //        var b = userShifts.Select(x => x.ShiftId).FirstOrDefault();
                //        a. = b;
                //        _dbContext.SaveChanges();
                //    }
                //    return
                //     existingProject;
                //}
            }
            
            
            return null;

        }

        public async Task<Projects> GetProjectById(Guid projectId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == projectId);

            if (project == null)
            {
                return null;
            }
            return project;
        }

        public async Task<IEnumerable<Projects>> GetProjectsData()
        {
            return await _dbContext.Projects.ToListAsync();
        }

        public object UserShift(Guid projectId)
        {
            throw new NotImplementedException();
        }
    }
}
