namespace ET_ShiftManagementSystem.Models.UserModel
{
    public class GetUserRequest
    {
        public string username { get; set; }

        public string? Role { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string? ContactNumber { get; set; }

        public string? AlternateContactNumber { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
