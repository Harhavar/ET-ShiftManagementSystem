﻿using System.Globalization;

namespace ET_ShiftManagementSystem.Models.UserModel
{
    public class UserDto
    {
        public Guid id { get; set; }

        public string username { get; set; }

        public string Email { get; set; }

        public string password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string ContactNumber { get; set; }

        public string AlternateContactNumber { get; set; }

        public Guid? TenateID { get; set; }

        public string? Role { get; set; }
    }
}