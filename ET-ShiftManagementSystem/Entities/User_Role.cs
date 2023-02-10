
using ET_ShiftManagementSystem.Entities;
using ShiftMgtDbContext.Entities;
using System.Data;

namespace ET_ShiftManagementSystem.Entities
{
    public class User_Role
    {
        public Guid Id { get; set; }

        public Guid Userid { get; set; }

        public User User { get; set; }

        public Guid RoleId { get; set; }

        public Role Role { get; set; }
    }
}
