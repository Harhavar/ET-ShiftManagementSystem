using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using ShiftMgtDbContext.Entities;
using ET_ShiftManagementSystem.Models.ShiftModel;
using Pipelines.Sockets.Unofficial.Arenas;

namespace ET_ShiftManagementSystem.Servises
{
    public interface IShiftServices
    {
        public List<Entities.AddShiftRequest> GetAllShiftAsync();
        public List<Entities.AddShiftRequest> GetAllShift(Guid TenatID);
        Entities.AddShiftRequest GetShiftDetails(Guid ShiftId);

        Task<Entities.AddShiftRequest> AddSift(Guid tenentId , Entities.AddShiftRequest shift);
        Task<Entities.AddShiftRequest> GetShiftById(Guid ShiftId);

        Task<List<User>> userShifts(Guid ProjectId);
        Task<List<User>> userShiftsDetails(Guid ShiftId);
        Task<List<Entities.AddShiftRequest>> UserShiftName(Guid ProjectId);

        Task<Entities.AddShiftRequest> UpdateShiftAsync(Guid id , UpdateShiftRequest shift);

         public Entities.AddShiftRequest DeleteShiftAsync(Guid ShiftId);
        void Update(List<UserShift> projectUser);
         public List<UserShift> UpdateExisting(Guid ProjectId ,List<UserShift> projectUser);
        void UpdateExisting(Guid ProjectID , List<addShift> projectUser);
    }
    public class ShiftServices : IShiftServices
    {
        private readonly ShiftManagementDbContext _dbContext;
        public ShiftServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Entities.AddShiftRequest> GetAllShiftAsync()
        {
            return  _dbContext.Shifts.ToList();
        }

        public async Task<Entities.AddShiftRequest> GetShiftById(Guid ShiftId)
        {
           var responce = await _dbContext.Shifts.FirstOrDefaultAsync(x => x.ShiftID == ShiftId);
            return responce;
        }

        public Entities.AddShiftRequest GetShiftDetails(Guid shiftId)
        {
            return _dbContext.Shifts.FirstOrDefault(x => x.ShiftID == shiftId);
        }


        public async Task<Entities.AddShiftRequest> AddSift(Guid tenentId , Entities.AddShiftRequest shift)
        {
            shift.ShiftID = Guid.NewGuid();
            shift.TenantId = tenentId;
            _dbContext.Shifts.Add(shift);
            _dbContext.SaveChangesAsync();
            return shift;
        }

        public async Task<Entities.AddShiftRequest> UpdateShiftAsync( Guid id , UpdateShiftRequest shift)
        {
            var ExistingShift = await _dbContext.Shifts.FirstOrDefaultAsync(x => x.ShiftID == id);

            if (ExistingShift != null)
            {
                ExistingShift.ShiftName = shift.ShiftName;
                ExistingShift.StartTime = shift.StartTime;
                ExistingShift.EndTime = shift.EndTime;
                await _dbContext.SaveChangesAsync();
            }
            return null;
        }


        public Entities.AddShiftRequest DeleteShiftAsync(Guid ShiftId)
        {
            var Shift = _dbContext.Shifts.FirstOrDefault(x => x.ShiftID == ShiftId);

            if (Shift != null)
            {
                _dbContext.Shifts.Remove(Shift);
                _dbContext.SaveChanges();
                return Shift;
            }
            return null;

        }

        public void Update(List<UserShift> projectUser)
        {
            foreach (var user in projectUser)
            {
                _dbContext.UserShifts.Add(user);
                _dbContext.SaveChanges();
            }

        }
        public List<UserShift> UpdateExisting( Guid ProjectId , List<UserShift> projectUser)
        {
            var existingUserShift = _dbContext.UserShifts.Where(x => x.ProjectId == ProjectId).ToList();

            if (existingUserShift.Any())
            {

                foreach (var item in existingUserShift)
                {
                    var a = existingUserShift.Where(x => x.ProjectId == ProjectId).Select(x => x.ShiftId).FirstOrDefault();
                    var b = projectUser.Select(x => x.ShiftId).FirstOrDefault();
                    var c = existingUserShift.Where(x => x.ProjectId == ProjectId).Select(x => x.UserId).FirstOrDefault();
                    var d = projectUser.Select(x => x.UserId).FirstOrDefault();
                    a = b;
                    c = d;
                     _dbContext.SaveChanges();
                }
                return existingUserShift;
            }
            return null;
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

        public List<Entities.AddShiftRequest> GetAllShift(Guid TenatID)
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

        public async Task<List<Entities.AddShiftRequest>> UserShiftName(Guid ProjectId)
        {
            var ShiftIds = _dbContext.UserShifts.Where(x => x.ProjectId == ProjectId).Select(x => x.ShiftId).ToList();
            var ShiftName = new List<Entities.AddShiftRequest>();
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
