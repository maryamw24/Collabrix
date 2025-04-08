using System.ComponentModel.DataAnnotations.Schema;

namespace Collabrix.Models
{

    [Table("TaskFileAttachments")]
    public class TaskFileAttachment
    {
        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [ForeignKey("FileAttachment")]
        public int AttachmentId { get; set; }
    }
}