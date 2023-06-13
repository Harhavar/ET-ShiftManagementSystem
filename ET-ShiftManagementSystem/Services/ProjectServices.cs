using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.ProjectsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pipelines.Sockets.Unofficial.Arenas;
using SendGrid.Helpers.Mail;

namespace ET_ShiftManagementSystem.Services
{
    public interface IProjectServices
    {
        public Task<Projects> AddProject(Guid tenantId, Projects project);
        public Task<Projects> DeleteProject(Guid projectId);
        public Task EditProject(Guid ProjectId, EditProjectRequest project);
        public Projects GetProjectById(Guid projectId);
        public List<Projects> GetProjectsData();
        public List<Projects> GetProjects(Guid UserId);
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
            project.Status = "Active";
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
            var activity = new Activity
            {
                ActivityId = Guid.NewGuid(),
                ProjectName = project.Name,
                Action = "Project Added",
                Message = "",
                UserName = "",
                TenetId = tenantId,
                Timestamp = DateTime.Now,
            };
            _dbContext.Activities.Add(activity);
            _dbContext.SaveChanges();
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

        public  Task EditProject(Guid ProjectId, EditProjectRequest project)
        {
            var existingProject =  _dbContext.Projects.FirstOrDefault(x => x.ProjectId == ProjectId);

            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.LastModifiedDate = DateTime.Now;
                existingProject.Status = "Active";
                _dbContext.SaveChanges();

                return Task.CompletedTask;

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

        public List<Projects> GetProjects(Guid UserId)
        {
            var ProjectId = _dbContext.UserShifts.Where(x => x.UserId== UserId).Select(x => x.ProjectId).ToList();
            if (ProjectId == null)
            {
                return null;
            }
            var Projects = new List<Projects>();

            ProjectId.ToList().ForEach(id =>
            {
                var project = _dbContext.Projects.Where(x => x.ProjectId == id).FirstOrDefault();
                
                Projects.Add(project);

            });
            
            if (Projects == null)
            {
                return null;
            }
            
            return Projects;
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
