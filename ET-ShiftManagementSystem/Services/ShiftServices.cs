using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http;
using ET_ShiftManagementSystem.Models.ShiftModel;
using Pipelines.Sockets.Unofficial.Arenas;
using Xunit.Extensions.AssertExtensions;

namespace ET_ShiftManagementSystem.Servises
{
    public interface IShiftServices
    {
        public Task<IEnumerable<Shift>> GetAllShiftAsync();
        public List<Shift> GetAllShift(Guid TenatID);
        Shift GetShiftDetails(Guid ShiftId);

        Task<Shift> AddSift(Shift shift);
        Task<Shift> GetShiftById(Guid ShiftId);

        Task<List<User>> userShifts(Guid ProjectId);
        Task<List<User>> userShiftsDetails(Guid ShiftId);
        Task<List<Shift>> UserShiftName(Guid ProjectId);

        void UpdateShiftAsync(Shift shift);

        Task<bool> DeleteShiftAsync(Guid ShiftId);
        void Update(List<UserShift> projectUser);
        void UpdateExisting(Guid ProjectID , List<addShift> projectUser);
    }
    public class ShiftServices : IShiftServices
    {
        private readonly ShiftManagementDbContext _dbContext;
        public ShiftServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Shift>> GetAllShiftAsync()
        {
            return await _dbContext.Shifts.ToListAsync();
        }

        public async Task<Shift> GetShiftById(Guid ShiftId)
        {
            return await _dbContext.Shifts.FirstOrDefaultAsync(x => x.ShiftID == ShiftId);
        }

        public Shift GetShiftDetails(Guid shiftId)
        {
            return _dbContext.Shifts.FirstOrDefault(x => x.ShiftID == shiftId);
        }


        public async Task<Shift> AddSift(Shift shift)
        {
            _dbContext.Shifts.Add(shift);
            await _dbContext.SaveChangesAsync();
            return shift;
        }

        public async void UpdateShiftAsync(Shift shift)
        {
            var ExistingShift = await _dbContext.Shifts.FirstOrDefaultAsync(x => x.ShiftID == shift.ShiftID);
            if (ExistingShift != null)
            {
                ExistingShift.ShiftName = shift.ShiftName;
                ExistingShift.StartTime = shift.StartTime;
                ExistingShift.EndTime = shift.EndTime;
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<bool> DeleteShiftAsync(Guid ShiftId)
        {
            var Shift = await _dbContext.Shifts.FirstOrDefaultAsync(x => x.ShiftID == ShiftId);

            if (Shift != null)
            {
                _dbContext.Shifts.Remove(Shift);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public void Update(List<UserShift> projectUser)
        {
            foreach (var user in projectUser)
            {
                _dbContext.UserShifts.Add(user);
                _dbContext.SaveChanges();
            }


        }
        public async  void UpdateExisting(Guid ProjectID, List<addShift> projectUser)
        {
            var existingProject =  await _dbContext.UserShifts.Where(x => x.ProjectId == ProjectID).Select(x => x.ProjectId).ToListAsync();

            if(existingProject.Count > 0)
            {
                for (int i = 0; i < existingProject.Count; i++)
                {
                    foreach (var itemId in existingProject)
                    {
                        var userDetails = _dbContext.UserShifts.FirstOrDefault(a => a.ProjectId == itemId);
                        userDetails.ShiftId = projectUser.Select(x => x.ShiftId).FirstOrDefault();
                        userDetails.UserId = projectUser.Select(x => x.UserId).FirstOrDefault();
                        _dbContext.SaveChanges();
                    };
                
                } 
            }

        }

        public List<Entities.Shift> GetAllShift(Guid TenatID)
        {
            return _dbContext.Shifts.Where(x => x.TenantId == TenatID).ToList();
        }

        public async Task<List<User>> userShifts(Guid ProjectId)
        {
            var userIds = _dbContext.UserShifts.Where(x => x.ProjectId == ProjectId).Select(x => x.UserId).ToList();

            var users = new List<User>();
            foreach (var UserId in userIds)
            {
                var userDetails = _dbContext.users.FirstOrDefault(a => a.id == UserId);
                if (userDetails != null)
                    users.Add(userDetails);
            }
            return users;
        }

        public async Task<List<Shift>> UserShiftName(Guid ProjectId)
        {
            var ShiftIds = _dbContext.UserShifts.Where(x => x.ProjectId == ProjectId).Select(x => x.ShiftId).ToList();
            var ShiftName = new List<Shift>();
            foreach (var item in ShiftIds)
            {
                var ShiftDetails = _dbContext.Shifts.FirstOrDefault(a => a.ShiftID == item);
                if (ShiftDetails != null)
                    ShiftName.Add(ShiftDetails);
            }
            return ShiftName;
        }

        public async Task<List<User>> userShiftsDetails(Guid ShiftId)
        {
            var userIds =  _dbContext.UserShifts.Where(x => x.ShiftId == ShiftId).Select(x => x.UserId).ToList();

            var users = new List<User>();
            foreach (var UserId in userIds)
            {
                var userDetails = _dbContext.users.FirstOrDefault(a => a.id == UserId);
                if (userDetails != null)
                    users.Add(userDetails);
            }
            return users;
        }

       

        public class UserShiftResponce
        {
            public List<ShiftNames> Shifts { get; set; }

            public List<ShiftUserDetails> Users { get; set; }
        };

        public class UserDetails
        {
            public List<UserDetailsInShift> Users { get; set; }
        }
    }
}
