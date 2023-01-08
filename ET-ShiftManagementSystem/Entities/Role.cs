using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<User_Role> UserRoles { get; set; }

    }
}
