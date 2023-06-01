namespace ET_ShiftManagementSystem.Models.UserModel
{
    public class UpdateUserProfileRequest
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        //public string? Role { get; set; }
        //public bool IsActive { get; set; }

        public string ContactNumber { get; set; }

        public string AlternateContactNumber { get; set; }
    }
    public class UpdateUserRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        public string ContactNumber { get; set; }

        public string AlternateContactNumber { get; set; }
    }
}
