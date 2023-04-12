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
        public OrganizationRole PostRole(OrganizationRole organization);
        public OrganizationRole EditRoleRequest(Guid guid , OrganizationRole organization );
        public OrganizationRole DeleteRoleRequest(Guid guid);
        public List<GlobalRole> GetGlobalRoles();
        public GlobalRole GetGlobalRoles(Guid guid);
       
        public GlobalRole EditGlobalRoleRequest(Guid guid, GlobalRole organization);
        public GlobalRole DeleteGlobalRoleRequest(Guid guid);

    }

    public class OrganizationRoleServices : IOrganizationRoleServices
    {
        private readonly ShiftManagementDbContext _dbContext;

        public OrganizationRoleServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GlobalRole DeleteGlobalRoleRequest(Guid guid)
        {
            var delete =  _dbContext.GlobalRoles.FirstOrDefault(x => x.Id == guid);

            if (delete == null)
            {
                return null;
            }

            _dbContext.GlobalRoles.Remove(delete);
             _dbContext.SaveChanges();
            return delete;
        }

        public OrganizationRole DeleteRoleRequest(Guid guid)
        {
            var delete =  _dbContext.OrganizationRoles.FirstOrDefault(x => x.Id== guid);

            if(delete == null)
            {
                return null;
            }

            _dbContext.OrganizationRoles.Remove(delete);
            _dbContext.SaveChanges();
            return delete;

        }

        public  OrganizationRole EditRoleRequest(Guid guid, OrganizationRole organization)
        {
            
            var exisingRole = _dbContext.OrganizationRoles.FirstOrDefault(x => x.Id== guid);

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

        public GlobalRole EditGlobalRoleRequest(Guid guid, GlobalRole organization)
        {
            var exisingRole =  _dbContext.GlobalRoles.FirstOrDefault(x => x.Id == guid);

            
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

        public  GlobalRole GetGlobalRoles(Guid guid)
        {
            var role =  _dbContext.GlobalRoles.FirstOrDefault(x => x.Id
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

        public  OrganizationRole PostRole(OrganizationRole organization)
        {
            if (organization.RoleName == "" )
            {
                return null;
            }
            organization.Id = Guid.NewGuid();
            organization.RoleName = organization.RoleName;
            organization.Description= organization.Description;
            organization.LinkedPermission= organization.LinkedPermission;
            organization.RoleType = "Custom";
            organization.LastModifiedDate= DateTime.Now;
            organization.CreatedDate = DateTime.Now;
             _dbContext.OrganizationRoles.Add(organization);
             _dbContext.SaveChanges();
            return organization;
        }
    }
}
