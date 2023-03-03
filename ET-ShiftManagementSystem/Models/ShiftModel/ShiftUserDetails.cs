namespace ET_ShiftManagementSystem.Models.ShiftModel
{
    public class ShiftUserDetails
    {
        public string username { get; set; }
        
    }

    public class ShiftNames
    {
        public string ShiftName { get; set; }
    }

    public class UserDetailsInShift
    {
        public string username { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }

        public string? AlternateContactNumber { get; set; }
    }
}
