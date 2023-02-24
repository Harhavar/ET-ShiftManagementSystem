using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;

namespace ET_ShiftManagementSystem.Services
{
    public interface IRoleServices
    {
        public Role GetRole();
    }


    public class RoleServices : IRoleServices
    {
        private readonly ShiftManagementDbContext _dbContext;

        public RoleServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Role GetRole()
        {
            var role = _dbContext.roles.Where(x => x.Name == "SuperAdmin").FirstOrDefault();
             role = _dbContext.roles.Where(x => x.Name == "User").FirstOrDefault();
             role = _dbContext.roles.Where(x => x.Name == "Admin").FirstOrDefault();

            return role;
        }
    }
}
