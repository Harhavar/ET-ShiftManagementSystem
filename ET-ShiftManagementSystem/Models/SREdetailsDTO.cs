﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ET_ShiftManagementSystem.Models
{
    public class SREdetailsDTO
    {
       // public int Id { get; set; }

        public string SRE { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        //public bool IsActive { get; set; }

        //[ForeignKey("ProjectId")]
        //public int ProjectId { get; set; }
    }
}