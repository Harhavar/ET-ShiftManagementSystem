namespace ET_ShiftManagementSystem.Models.OrganizationRoleModel
{
    public class OrganizationRoleViewRequest
    {
        public string RoleName { get; set; }

        public string Description { get; set; }

        public string LinkedPermission { get; set; }

        public string RoleType { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
