using System.ComponentModel.DataAnnotations;

namespace ET_ShiftManagementSystem.Entities
{
    public class Projects
    {
        [Key]
        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public string Description  { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set;}

        public Guid TenentId { get; set; }

        public string Status { get; set; }
    }
}
