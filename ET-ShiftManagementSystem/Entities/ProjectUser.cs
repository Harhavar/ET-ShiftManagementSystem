using ShiftMgtDbContext.Entities;
using System.ComponentModel.DataAnnotations;

namespace ET_ShiftManagementSystem.Entities
{
    public class ProjectUser
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        //public User User { get; set; }

        public int ProjectId { get; set; }

        //public Project project { get; set; }
    }
}
