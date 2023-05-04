using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;

namespace ET_ShiftManagementSystem.Services
{
    public interface IPermissionServises
    {
       public List<Permission> GetPermissions();

        public Task<Permission> GetPermissionById(Guid id);
        public Task<Permission> AddPermission(Permission permission);
        public Task<OrgPermission> AddOrgPermission(OrgPermission permission);
        public Task<Permission> EditPermission(Guid id , Permission permission);
        public Task<OrgPermission> EditOrgPermission(Guid id , OrgPermission permission);

        public Permission DeletePermission(Guid id);
        public bool DeleteOrgPermission(Guid id);

        public List<OrgPermission> GetOrgPermissions();

        public OrgPermission GetOrgPermissionById(Guid id);

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
            if (permission == null)
            {
                return null;
            }
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

        public  List<Permission> GetPermissions()
        {
            var responce = shiftManagementDbContext.Permissions.ToList();
            if (responce != null)
            {
              return responce;

            }
            return null;
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
        public Permission DeletePermission(Guid id)
        {
            var deletePermission = shiftManagementDbContext.Permissions.FirstOrDefault(x => x.Id == id);

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

        
        public  List<OrgPermission> GetOrgPermissions()
        {
            return  shiftManagementDbContext.OrgPermissions.ToList();
        }

        public OrgPermission GetOrgPermissionById(Guid id)
        {
            var permission =  shiftManagementDbContext.OrgPermissions.FirstOrDefault(x => x.Id == id);

            if (permission == null)
            {
                return null;
            }

            return permission;
        }

        public async Task<OrgPermission> EditOrgPermission(Guid id, OrgPermission permission)
        {
            var existingPermission = await shiftManagementDbContext.OrgPermissions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingPermission == null)
            {
                return null;
            }


            existingPermission.Id = id;
            existingPermission.PermissionName = permission.PermissionName;
            existingPermission.Description = permission.Description;
            existingPermission.LastModifiedDate = DateTime.Now;

            await shiftManagementDbContext.SaveChangesAsync();


            return existingPermission;
        }

        public async  Task<OrgPermission> AddOrgPermission(OrgPermission permission)
        {
            if (permission == null)
            {
                return null;
            }
            permission.Id = Guid.NewGuid();
            permission.CreatedDate = DateTime.Now;
            permission.LastModifiedDate = DateTime.Now;
            permission.PermissionType = "Custom";
            await shiftManagementDbContext.OrgPermissions.AddAsync(permission);
            await shiftManagementDbContext.SaveChangesAsync();
            return permission;
        }

        public bool DeleteOrgPermission(Guid id)
        {
            var deletePermission = shiftManagementDbContext.OrgPermissions.FirstOrDefault(x => x.Id == id);

            if (deletePermission == null)
            {
                return false;

            }

             shiftManagementDbContext.OrgPermissions.Remove(deletePermission);
             shiftManagementDbContext.SaveChanges();
             return true;
        }
    }
}
