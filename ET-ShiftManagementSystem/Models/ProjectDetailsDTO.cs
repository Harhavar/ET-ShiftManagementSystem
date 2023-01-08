using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Nodes;

namespace ET_ShiftManagementSystem.Models
{

    public class ProjectDetailsDTO
    {
        public int ProjectDetailsID { get; set; }


        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsActive { get; set; }

        public int? ShiftID { get; set; }
        public int ProjectId { get; set; }


        public int UserID { get; set; }

        //navigation property 
        public IEnumerable<ProjectDetail> projectDetails { get; set; }
        //public Project project { get; set; }

        //public Shift shift { get; set; }
        
    }
    public class ProjectUser
    {
        public int  UserId { get; set; }
        public string UserName { get; set; } 

        public string Email { get; set; }

        public string FullName { get; set; }

        public int ShiftID { get; set; }

        public string ShiftName { get; set; }

    }

    public class ShiftDetails
    {

        public string ShiftName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class CommentDetailes
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }

        public string EmployeeName { get; set; }

        public int ShiftID { get; set; }

        public int UserID { get; set; }

        public string Shift { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    //public ProjectDetailsDTO()
    //    {
    //        ProjectUsers = new List<ProjectUser>();
    //        CommentDetiles = new List<CommentDetailes>();
    //       // ShiftDetails = new List<ShiftDetails>();
    //    }
    //    public string ProjectName { get; set; }

    //    public string Description { get; set; }

    //    public string ClientName { get; set; }


    //    public List<ProjectUser> ProjectUsers {get; set; }

    //    //public List<ShiftDetails> ShiftDetails { get; set; }

       // public List<CommentDetailes> CommentDetiles { get; set; }
      //  public int ProjectID { get;  set; }
}
