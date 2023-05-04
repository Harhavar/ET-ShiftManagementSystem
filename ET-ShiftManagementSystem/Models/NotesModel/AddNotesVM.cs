namespace ET_ShiftManagementSystem.Models.NotesModel
{
    public class AddNotesVM
    {
        public Guid ProjectId { get; set; }
        public string Text { get; set; }

        public IFormFile? FileDetails { get; set; }
    }
}
