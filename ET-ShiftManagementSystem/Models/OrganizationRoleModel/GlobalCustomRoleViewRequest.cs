namespace ET_ShiftManagementSystem.Models.OrganizationRoleModel
{
    public class GlobalCustomRoleViewRequest
    {
        public string RoleName { get; set; }

        public string Description { get; set; }

        public string LinkedPermission { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
