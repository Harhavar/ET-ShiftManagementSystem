using ET_ShiftManagementSystem.Models.AlertModel;
using Microsoft.Graph;

namespace ET_ShiftManagementSystem.Entities
{
    public class Alert
    {
        public Guid Id    { get; set; }    
        
        public string AlertName { get; set; }

        public DateTime TriggeredTime { get; set; }

        public string Description { get; set; }
        
        public  string RCA { get; set; }

        public string ReportedBy { get; set; }

        public string ReportedTo { get; set; }

        public DateTime CreatedDate { get; set;}

        public severityLevel severity { get; set; }

        public DateTime lastModifiedDate { get; set; }

        public string Status { get; set; }

        public Guid ProjectId { get; set; }

        public Guid TenantId { get; set; }
    }
}
