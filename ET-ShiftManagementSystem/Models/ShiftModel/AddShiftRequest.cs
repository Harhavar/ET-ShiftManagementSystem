﻿namespace ET_ShiftManagementSystem.Models.ShiftModel
{
    public class AddShiftRequest
    {
       //public Guid ShiftID { get; set; }

        public string ShiftName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}