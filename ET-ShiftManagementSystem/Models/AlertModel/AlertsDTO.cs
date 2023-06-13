namespace ET_ShiftManagementSystem.Models.AlertModel
{
    public class AlertsDTO
    {
        public Guid Id { get; set; }

        public string AlertName { get; set; }

        public DateTime TriggeredTime { get; set; }

        public string Description { get; set; }

        public string RCA { get; set; }

        public Guid ReportedBy { get; set; }

        public Guid ReportedTo { get; set; }

        public DateTime CreatedDate { get; set; }
        public int severity { get; set; }
        public DateTime lastModifiedDate { get; set; }

        public string Status { get; set; }

        public Guid ProjectId { get; set; }

        public Guid TenantId { get; set; }

        public string ReportedByName { get; set; }

        public string ReportedToName { get; set; }
    }
}
