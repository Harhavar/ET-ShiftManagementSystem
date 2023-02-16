using System.ComponentModel.DataAnnotations;

namespace ET_ShiftManagementSystem.Entities
{
    public class Permission
    {
        [Key]
        public Guid Id { get; set; }

        public string PermissionName { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
