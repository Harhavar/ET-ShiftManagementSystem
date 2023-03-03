﻿using System.Security.Cryptography.X509Certificates;

namespace ET_ShiftManagementSystem.Models.Authmodel
{
    public class RegisterRequest
    {
        //public Guid id { get; set; }

        public string username { get; set; }

        public string Email { get; set; }

        public string password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactNumber { get; set; }

        public string AlternateContactNumber { get; set; }


        public Guid TenateID { get; set; }
    }
}
