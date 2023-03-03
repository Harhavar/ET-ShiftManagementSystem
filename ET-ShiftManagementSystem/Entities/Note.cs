namespace ET_ShiftManagementSystem.Entities
{
    public class Note
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public Guid ProjectId { get; set; }

        public string Text { get; set; }

        public string FileName { get; set; }

        public byte[] FileData { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
