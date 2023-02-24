using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ET_ShiftManagementSystem.Models.DocModel;

namespace ET_ShiftManagementSystem.Entities
{
    [Table("FileDetails")]
    public class FileDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public FileType? FileType { get; set; }

        public DateTime? UpdateDate { get; set; }

        public Guid TenentID { get; set; }
    }
}
