﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftMgtDbContext.Entities
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public const string User = "User";
        
        public const string Admin = "Admin";

    }
}
