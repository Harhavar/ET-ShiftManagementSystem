namespace ET_ShiftManagementSystem.Models.TaskModel
{
    public class TaskDTO
    {
        public int Id { get; set; }

        public string TaskComment { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool isActive { get; set; }
    }
}
