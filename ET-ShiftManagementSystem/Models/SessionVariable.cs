namespace ET_ShiftManagementSystem.Models
{
    public class SessionVariable
    {
        public const string SessionKeyUserName = "SessionKeyUserName";
        public const string SessionKeySessionId = "SessionKeySessionId";
    }

    public enum SessionKeyEnum
    {
        SessionKeyUserName = 0,
        SessionKeySessionId = 1
    }
}
