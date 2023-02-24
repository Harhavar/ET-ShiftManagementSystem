using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace ET_ShiftManagementSystem.Services
{
    public interface IProjectServices
    {
        public List<Projects> GetProject();
    }
    public class ProjectServices : IProjectServices
    {
        private readonly ShiftManagementDbContext dbContext;

        public ProjectServices(ShiftManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Projects> GetProject()
        {
            return  dbContext.Projects.ToList();
        }
    }
}
