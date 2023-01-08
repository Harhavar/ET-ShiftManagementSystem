using System.ComponentModel.DataAnnotations;

namespace ET_ShiftManagementSystem.Models
{
    public class forgotPasswordRequest
    { 
    //    [Required(ErrorMessage = " User name is required ")]
    //    public string username { get; set; }


    //    [Required(ErrorMessage = "new password is required ")]
    //    public string NewPassword { get; set; }

        [Required(ErrorMessage = "email is required ")]
        public string Email { get; set; }


        //[Required(ErrorMessage = "confirm password is required ")]
        //public string ConfirmPassword { get; set; }

        //public string Token { get; set; }
    }
}
