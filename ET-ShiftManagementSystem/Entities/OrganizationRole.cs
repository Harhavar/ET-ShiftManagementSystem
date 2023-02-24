namespace ET_ShiftManagementSystem.Entities
{
    public class OrganizationRole
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public string LinkedPermission { get; set; }

        public string RoleType { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
