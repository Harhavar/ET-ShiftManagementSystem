namespace ET_ShiftManagementSystem.Models
{
    public class ResetPasswordViewModel
    {
        public Guid UserId { get; set; }

        public string Email {  get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
