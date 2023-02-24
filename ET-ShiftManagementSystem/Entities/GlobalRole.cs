namespace ET_ShiftManagementSystem.Entities
{
    public class GlobalRole
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public string LinkedPermission { get; set; }


        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
