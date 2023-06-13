namespace ET_ShiftManagementSystem.Models.ProjectsModel
{
    public class ProjectDetailsRequest
    {
        public Guid TenantId { get; set; }

        public Guid ProjectId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
    public class ProjectIds
    {
        public Guid ProjectId { get; set; }
    }
}
