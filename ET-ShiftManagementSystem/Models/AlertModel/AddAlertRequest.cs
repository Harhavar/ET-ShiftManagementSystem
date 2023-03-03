namespace ET_ShiftManagementSystem.Models.AlertModel
{
    public class AlertRequest
    {
        public string AlertName { get; set; }

        public string Status { get; set; }
        public DateTime TriggeredTime { get; set; }

        public string Description { get; set; }

        
        

        public string ReportedTo { get; set; }

       

        
    }
    public class AddAlertRequest
    {
       public AlertRequest alertRequest { get; set; }
        public severityLevel severity { get; set; }
    }

}
