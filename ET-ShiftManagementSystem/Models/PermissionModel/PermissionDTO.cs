namespace ET_ShiftManagementSystem.Models.PermissionModel
{
    public class PermissionDTO
    {
        public Guid Id { get; set; }

        public string PermissionName { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
