using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ET_ShiftManagementSystem.Services
{
    public interface IorganizationServices
    {
        public Task<IEnumerable<Organization>> GetOrganizationData();

        public Task<Organization> AddOrgnization(Organization organization);

        public Task<Organization> GetOrgByID(Guid id);

        public Task<Organization> UpdateOrganization(Guid id , Organization organization);

        public Task<Organization> DeleteOrganization(Guid id);
    }
    public class organizationServices : IorganizationServices
    {
        private readonly ShiftManagementDbContext shiftManagementDb;

        public organizationServices(ShiftManagementDbContext shiftManagementDb)
        {
            this.shiftManagementDb = shiftManagementDb;
        }

        public async Task<Organization> AddOrgnization(Organization organization)
        {
            organization.TenentID= Guid.NewGuid();
            organization.CreatedDate= DateTime.Now;
            organization.LastModifiedDate= DateTime.Now;
            await shiftManagementDb.Organizations.AddAsync(organization);
            await shiftManagementDb.SaveChangesAsync();
            return organization;

        }

        public async Task<Organization> DeleteOrganization(Guid id)
        {
           var delete =  await shiftManagementDb.Organizations.FirstOrDefaultAsync(x => x.TenentID == id);

            if (delete == null)
            {
                return null;
            }

             shiftManagementDb.Organizations.Remove(delete);
            shiftManagementDb.SaveChanges();
            return delete;
        }

        public   async Task<IEnumerable<Organization>> GetOrganizationData()
        {
            return await shiftManagementDb.Organizations.ToListAsync();
        }

        public async Task<Organization> GetOrgByID(Guid id)
        {
            return await shiftManagementDb.Organizations.FirstOrDefaultAsync( x => x.TenentID==id);
        }

        public async Task<Organization> UpdateOrganization(Guid id, Organization organization)
        {
            var ExistingOrganization = shiftManagementDb.Organizations.FirstOrDefault(x => x.TenentID == id);

            if (ExistingOrganization == null)
            {
                return null;
            }

            ExistingOrganization.OrganizationLogo = organization.OrganizationLogo;
            ExistingOrganization.OrganizationName = organization.OrganizationName;
            ExistingOrganization.StreetLandmark = organization.StreetLandmark;
            ExistingOrganization.PhoneNumber = organization.PhoneNumber;
            ExistingOrganization.HouseBuildingNumber = organization.HouseBuildingNumber;
            ExistingOrganization.Country= organization.Country;
            ExistingOrganization.PhoneNumber= organization.PhoneNumber;
            ExistingOrganization.Adminemailaddress= organization.Adminemailaddress;
            ExistingOrganization.Adminfullname= organization.Adminfullname;
            ExistingOrganization.EmailAddress= organization.EmailAddress;
            ExistingOrganization.CityTown= organization.CityTown;
            ExistingOrganization.StateProvince= organization.StateProvince;
            ExistingOrganization.LastModifiedDate = DateTime.Now;

            await shiftManagementDb.SaveChangesAsync();

            return ExistingOrganization;

        }
    }
}
