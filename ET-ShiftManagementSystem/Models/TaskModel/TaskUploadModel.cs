﻿using ET_ShiftManagementSystem.Models.DocModel;

namespace ET_ShiftManagementSystem.Models.TaskModel
{
    public class TaskUploadModel
    {
        public Guid ProjectId { get; set; }
        public string Text { get; set; }

        public Actions Actions { get; set; }
        public Guid? TaskGivenTo { get; set; }
        public IFormFile? FileDetails { get; set; }
         public FileType? FileType { get; set; }
        public DateTime DueDate { get; set; }
    }
}
