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
        public Guid ShiftID { get; set; }

        public string ShiftName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public Guid TenantId { get; set; }

    }
}
