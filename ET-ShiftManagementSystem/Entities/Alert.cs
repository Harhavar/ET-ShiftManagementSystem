namespace ET_ShiftManagementSystem.Entities
{
    public class Alert
    {
        public int Id    { get; set; }    
        
        public string AlertName { get; set; }

        public DateTime TriggeredTime { get; set; }

        public string Description { get; set; }
        
        public  string RCA { get; set; }

        public string ReportedBy { get; set; }

        public string ReportedTo { get; set; }

        public DateTime CreatedDate { get; set;}
    }
}
