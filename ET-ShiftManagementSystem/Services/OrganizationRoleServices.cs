using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;

namespace ET_ShiftManagementSystem.Services
{
    public interface IOrganizationRoleServices
    {
        public List<OrganizationRole> GetRoles();
        public OrganizationRole GetRoles(Guid guid);
        public Task<OrganizationRole> PostRole(OrganizationRole organization);
        public Task<OrganizationRole> EditRoleRequest(Guid guid , OrganizationRole organization );
        public Task<OrganizationRole> DeleteRoleRequest(Guid guid);
        public List<GlobalRole> GetGlobalRoles();
        public Task<GlobalRole> GetGlobalRoles(Guid guid);
       
        public Task<GlobalRole> EditGlobalRoleRequest(Guid guid, GlobalRole organization);
        public Task<GlobalRole> DeleteGlobalRoleRequest(Guid guid);

    }

    public class OrganizationRoleServices : IOrganizationRoleServices
    {
        private readonly ShiftManagementDbContext _dbContext;

        public OrganizationRoleServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GlobalRole> DeleteGlobalRoleRequest(Guid guid)
        {
            var delete = await _dbContext.GlobalRoles.FirstOrDefaultAsync(x => x.Id == guid);

            if (delete == null)
            {
                return null;
            }

            _dbContext.GlobalRoles.Remove(delete);
            await _dbContext.SaveChangesAsync();
            return delete;
        }

        public async Task<OrganizationRole> DeleteRoleRequest(Guid guid)
        {
            var delete = await _dbContext.OrganizationRoles.FirstOrDefaultAsync(x => x.Id== guid);

            if(delete == null)
            {
                return null;
            }

            _dbContext.OrganizationRoles.Remove(delete);
            await _dbContext.SaveChangesAsync();
            return delete;

        }

        public async Task<OrganizationRole> EditRoleRequest(Guid guid, OrganizationRole organization)
        {
            //var exist = await _dbContext.OrganizationRoles.Where(x => x.Id == 
            var exisingRole =await _dbContext.OrganizationRoles.FirstOrDefaultAsync(x => x.Id== guid);

            //if(exisingRole.Id == 95A02FB3-A4B2-441C-ADB4-EC1189CF12C7  || exisingRole.Id ==  "B51D5C78-C2FD-4EF6-9316-D746214AB7E6")
            //{
            //    return null;
            //}
            if(exisingRole == null)
            {
                return null;
            }

            exisingRole.RoleName= organization.RoleName;
            exisingRole.Description= organization.Description;
            exisingRole.LinkedPermission= organization.LinkedPermission;
            exisingRole.LastModifiedDate = DateTime.Now;

            _dbContext.SaveChanges();
            return exisingRole;
        }

        public async Task<GlobalRole> EditGlobalRoleRequest(Guid guid, GlobalRole organization)
        {
            var exisingRole = await _dbContext.GlobalRoles.FirstOrDefaultAsync(x => x.Id == guid);

            
            if (exisingRole == null)
            {
                return null;
            }

            exisingRole.RoleName = organization.RoleName;
            exisingRole.Description = organization.Description;
            exisingRole.LinkedPermission = organization.LinkedPermission;
            exisingRole.LastModifiedDate = DateTime.Now;

            _dbContext.SaveChanges();
            return exisingRole;
        }

        public  List<GlobalRole> GetGlobalRoles()
        {
            return  _dbContext.GlobalRoles.ToList();
        }

        public async Task<GlobalRole> GetGlobalRoles(Guid guid)
        {
            var role = await _dbContext.GlobalRoles.FirstOrDefaultAsync(x => x.Id
              == guid);
            return role;
        }

        public  List<OrganizationRole> GetRoles()
        {
            return  _dbContext.OrganizationRoles.ToList();
        }

        public OrganizationRole GetRoles(Guid guid)
        {
            var role = _dbContext.OrganizationRoles.FirstOrDefault(x => x.Id
             == guid);
            return role;
        }

        public async Task<OrganizationRole> PostRole(OrganizationRole organization)
        {
            organization.Id = Guid.NewGuid();
            organization.RoleName = organization.RoleName;
            organization.Description= organization.Description;
            organization.LinkedPermission= organization.LinkedPermission;
            organization.RoleType = "Custom";
            organization.LastModifiedDate= DateTime.Now;
            organization.CreatedDate = DateTime.Now;
            await _dbContext.OrganizationRoles.AddAsync(organization);
            await _dbContext.SaveChangesAsync();
            return organization;
        }
    }
}
