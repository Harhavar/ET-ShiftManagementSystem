using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Entities
{
    public class UserCredential
    {
        [Key]
        public int UserID { get; set; }

        //public int LoginId { get; set; }
        public string UserName { get; set; } = string.Empty; 

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Password { get; set; }

       // public string RoleName { get; set; }

        //public UserRole UserRoles { get; set; }
    }
}
