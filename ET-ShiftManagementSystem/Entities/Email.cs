using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Entities
{
    public class Email
    {
        [Key]
        public string to { get; set; } = string.Empty;

        public string subject { get; set; } = string.Empty;

        public string body { get; set; } = string.Empty;
    }
}
