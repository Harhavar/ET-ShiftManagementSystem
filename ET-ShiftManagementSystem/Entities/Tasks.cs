using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Models.TaskModel;

namespace ET_ShiftManagementSystem.Entities
{
    public class Tasks
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public string Text { get; set; }

        public Actions? Actions { get; set; }

        public DateTime DueDate { get; set; }

        public string? DueComment { get; set; }

        public string? FileName { get; set; }

        public FileType? FileType { get; set; }

        public byte[]? FileData { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public string TaskGivenTo { get; set; }

        public List<TaskComment> TaskComments { get; set; }
    }
}
