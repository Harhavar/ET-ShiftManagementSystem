using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace ET_ShiftManagementSystem.Servises
{
    public interface ISREDetiles
    {
        Task<IEnumerable<SREDetaile>> GetAllSRE();

        long AddSRE(SREDetaile sre);

        Task<SREDetaile> UpdateSRE(int Id , SREDetaile sre);

        Task<SREDetaile> DeleteSRE(int Id);
    }
    public class SREservices : ISREDetiles
    {
        private readonly ShiftManagementDbContext _shiftManagementDb;

        public SREservices(ShiftManagementDbContext shiftManagementDb)
        {
            _shiftManagementDb = shiftManagementDb;
        }

        public long AddSRE(SREDetaile sre)
        {
            _shiftManagementDb.SREDetiles.Add(sre);
            _shiftManagementDb.SaveChanges();
            return sre.Id;
        }

        public async Task<SREDetaile> DeleteSRE(int Id)
        {
            var SRE = await _shiftManagementDb.SREDetiles.FirstOrDefaultAsync(sre => sre.Id == Id);

            if (SRE == null)
            {
                return null;
            }

            _shiftManagementDb.SREDetiles.Remove(SRE);
            _shiftManagementDb.SaveChangesAsync();

            return SRE;
        }

        public async Task<IEnumerable<SREDetaile>> GetAllSRE()
        {
            return await _shiftManagementDb.SREDetiles.ToListAsync();
        }

        public async  Task<SREDetaile> UpdateSRE(int Id , SREDetaile sre)
        {
            var abc = await _shiftManagementDb.SREDetiles.FirstOrDefaultAsync(x => x.Id == Id);

            if (abc == null)
            {
                return null;
            }

            abc.SRE = sre.SRE;
            abc.MobileNumber= sre.MobileNumber;
            abc.Email   = sre.Email;
            abc.IsActive= sre.IsActive;
           
            
            await _shiftManagementDb.SaveChangesAsync();

            return abc;
        }

    }
}
