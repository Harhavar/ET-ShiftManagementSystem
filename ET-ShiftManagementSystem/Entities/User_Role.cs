//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShiftMgtDbContext.Entities
//{
//    public class UserRole
//    {
//        [Key]
//        public int RoleId { get; set; }

//        public string RoleName { get; set; }

//        public const string User = "User";

//        public const string Admin = "Admin";

//    }
//}
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
