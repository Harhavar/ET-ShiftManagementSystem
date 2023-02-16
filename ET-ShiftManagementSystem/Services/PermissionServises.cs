using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;

namespace ET_ShiftManagementSystem.Services
{
    public interface IPermissionServises
    {
       public Task<IEnumerable<Permission>> GetPermissions();

        public Task<Permission> GetPermissionById(Guid id);
        public Task<Permission> AddPermission(Permission permission);
        public Task<Permission> EditPermission(Guid id , Permission permission);

        public Task<Permission> DeletePermission(Guid id);
    }
    public class PermissionServises : IPermissionServises
    {
        private readonly ShiftManagementDbContext shiftManagementDbContext;

        public PermissionServises(ShiftManagementDbContext shiftManagementDbContext)
        {
            this.shiftManagementDbContext = shiftManagementDbContext;
        }

        public async  Task<Permission> EditPermission(Guid id, Permission permission)
        {
           var existingPermission = await shiftManagementDbContext.Permissions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingPermission == null)
            {
                return null;
            }


            existingPermission.Id= id;
            existingPermission.PermissionName = permission.PermissionName;
            existingPermission.Description = permission.Description;
            existingPermission.LastModifiedDate = DateTime.Now;

            await shiftManagementDbContext.SaveChangesAsync();
            

            return existingPermission;

        }

        public async Task<IEnumerable<Permission>> GetPermissions()
        {
            return await shiftManagementDbContext.Permissions.ToListAsync();
        }
        public async Task<Permission> GetPermissionById(Guid id)
        {
            var permission = await shiftManagementDbContext.Permissions.FirstOrDefaultAsync(x => x.Id==id);

            if (permission == null)
            {
                return null;
            }

            return permission;
        }
        public async Task<Permission> DeletePermission(Guid id)
        {
            var deletePermission = await shiftManagementDbContext.Permissions.FirstOrDefaultAsync(x => x.Id == id);

            if(deletePermission == null )
            {
                return null;
                
            }

            shiftManagementDbContext.Permissions.Remove(deletePermission);
            shiftManagementDbContext.SaveChanges();
            return deletePermission;
        }

        public async Task<Permission> AddPermission(Permission permission)
        {
            if(permission == null)
            {
                return null;
            }
            permission.Id = Guid.NewGuid();
            permission.CreatedDate= DateTime.Now;
            permission.LastModifiedDate= DateTime.Now;
           await shiftManagementDbContext.Permissions.AddAsync(permission);
           await shiftManagementDbContext.SaveChangesAsync();
            return permission;
        }
    }
}
