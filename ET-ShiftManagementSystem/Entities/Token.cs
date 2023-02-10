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
    public class Token
    {
        [Key]
        public int Id { get; set; }

        public string? UserToken { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool? TokenUsed { get; set; }

        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }

        public string? Useremail { get; set;}

    }
}
