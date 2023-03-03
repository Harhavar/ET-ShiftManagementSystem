using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Entities
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public string ClientName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set;}
         
        public string ModifieBy { get; set; }

        public DateTime ModifieDate { get;set; }

        public bool IsActive { get; set; }


    }
}
