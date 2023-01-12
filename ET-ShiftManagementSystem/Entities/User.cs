using ET_ShiftManagementSystem.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftMgtDbContext.Entities
{
    public class User
    {
        public Guid id { get; set; }

        public string username { get; set; }

        public string Email { get; set; }               

        public string password { get; set; }

        [NotMapped]
        public List<string> Roles { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; } /*= string.Empty;*/

        public bool IsActive { get; set; }

        public List<User_Role> UserRoles { get; set; }


        public string? ContactNumber { get; set; }

        public string? AlternateContactNumber { get; set; }

        //navigaion property 
        //[ForeignKey("ProjectId")]
        //public int ProjectId { get; set; }

        //public Project Project { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }
    }
}
