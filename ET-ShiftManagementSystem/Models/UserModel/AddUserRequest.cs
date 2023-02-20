namespace ET_ShiftManagementSystem.Models.UserModel
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string AlternateContactNumber { get; set; }
    }
}
