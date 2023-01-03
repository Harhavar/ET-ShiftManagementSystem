using System.ComponentModel.DataAnnotations;

namespace ET_ShiftManagementSystem.Models
{
    public class ChangePasswordModel
    {
        [Required( ErrorMessage = " User name is required ")]
        public string username { get; set; }

        [Required(ErrorMessage = "Current password is required ")]
        public string password { get; set; }

        [Required(ErrorMessage ="new password is required ")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage ="confirm password is required ")]
        public string  ConfirmPassword { get; set; }
    }
}
