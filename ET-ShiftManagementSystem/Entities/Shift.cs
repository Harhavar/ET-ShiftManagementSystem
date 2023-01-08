using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Entities
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }

        public string ShiftName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        //[ForeignKey("ShiftId")]
        //public virtual Comment? CommentDetail { get; set; }
    }
}
