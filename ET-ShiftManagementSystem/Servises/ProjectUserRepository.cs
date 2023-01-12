using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Servises
{
    public interface IProjectUserRepository
    {
        ProjectUser GetUserId(Guid userId);
        ProjectUser GetProjectId(int ProjectId);
        Project GetProject(int ProjectId);

        Task<ProjectUser> Update(ProjectUser user);
        ProjectUser Remove(ProjectUser user);

        //ProjectUser 
    }
    public class ProjectUserRepository : IProjectUserRepository
    {
        private readonly ShiftManagementDbContext shiftManagementDb;

        public ProjectUserRepository(ShiftManagementDbContext shiftManagementDb)
        {
            this.shiftManagementDb = shiftManagementDb;
        }
        
        public Project GetProject(int ProjectId)
        {
            return  shiftManagementDb.projects.AsNoTracking().FirstOrDefault(a => a.ProjectId == ProjectId);
        }

        public ProjectUser GetUserId(Guid userId)
        {
            return shiftManagementDb.projectUsers.FirstOrDefault(x => x.UserId == userId);
        }

        public ProjectUser GetProjectId(int ProjectId)
        {
            return shiftManagementDb.projectUsers.FirstOrDefault(x => x.ProjectId == ProjectId);
        }


        public ProjectUser Remove(ProjectUser user)
        {
            shiftManagementDb.projectUsers.Remove(user);
             shiftManagementDb.SaveChanges();

            return user;
        }

        public async Task<ProjectUser> Update(ProjectUser user)
        {
            shiftManagementDb.projectUsers.Add(user);
            await shiftManagementDb.SaveChangesAsync();

            return user;
        }

    }
}
