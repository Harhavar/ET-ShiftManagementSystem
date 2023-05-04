namespace ET_ShiftManagementSystem.Models.DocModel
{
    public class FileUploadModel
    {
        public Guid ProjectId { get; set; }
        public IFormFile FileDetails { get; set; }
        public FileType FileType { get; set; }

    }
}
