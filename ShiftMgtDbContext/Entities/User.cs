//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShiftMgtDbContext.Entities
//{
//    public class User
//    {
//        [Key]
//        public int UserId { get; set; }
//        public string UserName { get; set; }

//        public string Email { get; set; }

//        public string FullName { get; set; }

//        public DateTime DateOfBirth { get; set; }

//        public string Gender { get; set; }

//        public bool IsActive { get; set; }

//        [ForeignKey("UserID")]
//        public virtual ProjectDetail ProjectDetail { get; set; }

//        [ForeignKey("UserID")]
//        public virtual Comment CommentDetail { get; set; }

//        [ForeignKey("UserID")]
//        public virtual UserCredential UserCredential { get; set; }


//    }

//}
//using ShiftMgtDbContext.Entities;
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

        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public List<User_Role> UserRoles { get; set; }


        public int ContactNumber { get; set; }

        public int AlternateContactNumber { get; set; }

        //navigaion property 
        // public Project Project { get; set; }
    }
}
