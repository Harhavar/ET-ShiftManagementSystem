namespace ET_ShiftManagementSystem.Models.PermissionModel
{
    public class OrgPermissionDTO
    {
        public Guid Id { get; set; }

        public string PermissionName { get; set; }

        public string Description { get; set; }

        public string PermissionType { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
