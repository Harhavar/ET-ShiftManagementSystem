using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace ET_ShiftManagementSystem.Services
{
    public interface ITenateServices
    {
        Task<IEnumerable<Tenate>> GetTenates();

        Task<Tenate> AssignTenantID(string user);
        //Task<
    }
    public class TenateServices : ITenateServices
    {
        private readonly ShiftManagementDbContext shiftManagementDb;

        public TenateServices(ShiftManagementDbContext shiftManagementDb)
        {
            this.shiftManagementDb = shiftManagementDb;
        }

        public Task<Tenate> AssignTenantID(string user)
        {
            // var tenentId = shiftManagementDb.Tenates.Where(x => x.TenateId == );
            return null;
        }

        public async  Task<IEnumerable<Tenate>> GetTenates()
        {
            return await shiftManagementDb.Tenates.ToListAsync();
        }

        //public async Task<Tenate> increaseStatus()
        //{
        //    shiftManagementDb.Tenates.
        //}
    }

    public static class Tenants
    {
        public const string Internet = nameof(Internet);
        public const string Khalid = nameof(Khalid);

        public static IReadOnlyCollection<string> All = new[] { Internet, Khalid };

        public static string Find(string? value)
        {
            return All.FirstOrDefault(t => t.Equals(value?.Trim(), StringComparison.OrdinalIgnoreCase)) ?? Internet;
        }
    }

    public class TenantService : ITenantGetter, ITenantSetter
    {
        public string Tenant { get; private set; } = Tenants.Internet;

        public void SetTenant(string tenant)
        {
            Tenant = tenant;
        }
    }

    public interface ITenantGetter
    {
        string Tenant { get; }
    }

    public interface ITenantSetter
    {
        void SetTenant(string tenant);
    }
}


