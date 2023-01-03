using System.Security.Cryptography.X509Certificates;

namespace ET_ShiftManagementSystem.Models
{
    public class RegisterRequest
    {
        //public Guid id { get; set; }

        public string username { get; set; }

        public string Email { get; set; }

        public string password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public int ContactNumber { get; set; }

        public int AlternateContactNumber { get; set; }
    }
}
