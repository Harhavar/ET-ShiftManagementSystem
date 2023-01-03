namespace ET_ShiftManagementSystem.Models
{
    public class DocDTO
    {

        public int Id { get; set; }

        public byte[] Docs { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string modifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool isActive { get; set; }
    }
}
