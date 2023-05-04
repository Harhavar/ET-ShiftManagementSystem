namespace ET_ShiftManagementSystem.Entities
{
    public class Activity
    {
        public Guid ActivityId { get; set; }
        public Guid TenetId { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string ProjectName { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
       
    }

}
