namespace ET_ShiftManagementSystem.Models.ProjectsModel
{
    public class ProjectDetailsRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
