using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftMgtDbContext.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }

        public string CommentText { get; set; }

        [Required]
        public int ShiftID { get; set; }

        public int UserID { get; set; }

        public bool Shared { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
