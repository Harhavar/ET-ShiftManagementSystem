namespace ET_ShiftManagementSystem.Models.ProjectsModel
{
    public class ProjectsViewRequest
    {
        public Guid Id { get; set; }

        public Guid  TenantId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        

    }
}
