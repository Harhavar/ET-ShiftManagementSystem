using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }

        public string CommentText { get; set; }

        [Required]
        //it should automaticaly select by time 
        public int ShiftID { get; set; }

        //it should automaticaly select by who is using based on authentication 
        public int UserID { get; set; }

        public bool Shared { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
