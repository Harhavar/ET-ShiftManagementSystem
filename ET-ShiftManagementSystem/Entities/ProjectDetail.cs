using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Entities
{
    public class ProjectDetail
    {
        [Key]
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
        public Project project { get; set; }

        public Shift shift { get; set; }
    }
}
