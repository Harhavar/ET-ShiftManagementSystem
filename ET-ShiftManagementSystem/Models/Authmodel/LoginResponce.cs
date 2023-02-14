namespace ET_ShiftManagementSystem.Models.Authmodel
{
    public class LoginResponce
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }

}
