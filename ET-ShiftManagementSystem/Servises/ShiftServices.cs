using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ET_ShiftManagementSystem.Servises
{
    public interface IShiftServices
    {
        public Task<IEnumerable<Shift>> GetAllShiftAsync();
        Shift GetShiftDetails(int ShiftId);

        long AddSift(Shift shift);
        Task<Shift> GetShiftById(int ShiftId);

        Task<Shift> UpdateShiftAsync(int id, Shift shift);

        Task<Shift> DeleteShiftAsync(int ShiftId);
    }
    public class ShiftServices : IShiftServices
    {
        private readonly ShiftManagementDbContext _Shiftservices;
        public ShiftServices(ShiftManagementDbContext Shiftservices)
        {
            _Shiftservices = Shiftservices;
        }

        public async Task<IEnumerable<Shift>> GetAllShiftAsync()
        {
            return await _Shiftservices.Shifts.ToListAsync();
        }

        public async Task<Shift> GetShiftById(int ShiftId)
        {
            return await _Shiftservices.Shifts.FirstOrDefaultAsync(x => x.ShiftId == ShiftId);
        }

        public Shift GetShiftDetails(int shiftId)
        {
            return _Shiftservices.Shifts.FirstOrDefault(x => x.ShiftId == shiftId);
        }
        

        public long AddSift( Shift shift)
        {
            _Shiftservices.Shifts.Add(shift);
            _Shiftservices.SaveChanges();
            return shift.ShiftId;
        }

        public async Task<Shift> UpdateShiftAsync(int id, Shift shift)
        {
            var ExistingShift = await _Shiftservices.Shifts.FirstOrDefaultAsync(x => x.ShiftId == id);

            if (ExistingShift == null)
            {
                return null;

            }

            ExistingShift.ShiftId = shift.ShiftId;
            ExistingShift.ShiftName = shift.ShiftName;
            ExistingShift.StartTime = shift.StartTime;
            ExistingShift.EndTime = shift.EndTime;

            await _Shiftservices.SaveChangesAsync();


            return ExistingShift;


        }

      
        public async Task<Shift> DeleteShiftAsync(int ShiftId)
        {
            var Shift = await _Shiftservices.Shifts.FirstOrDefaultAsync(x => x.ShiftId == ShiftId);
            
            if (Shift == null)
            {
                return null;
            }


            _Shiftservices.Shifts.Remove(Shift);
            await _Shiftservices.SaveChangesAsync();
            return Shift;

        }
    }
}
