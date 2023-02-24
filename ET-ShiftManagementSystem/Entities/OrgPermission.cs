namespace ET_ShiftManagementSystem.Entities
{
    public class OrgPermission
    {
        public Guid Id { get; set; }

        public string PermissionName { get; set; }

        public string Description { get; set; }

        public string PermissionType { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
