using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("FileAttachments")]
    public class FileAttachment
    {
        [Key]
        public int AttachmentId { get; set; }

        [Required]
        public string FilePath { get; set; }

        [ForeignKey("UploadedByUser")]
        public int UploadedBy { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}