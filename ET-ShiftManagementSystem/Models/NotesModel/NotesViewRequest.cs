namespace ET_ShiftManagementSystem.Models.NotesModel
{
    public class NotesViewRequest
    {
        public string Text { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FileName { get; set; }

        public byte[] FileData { get; set; }
    }
}
