using System.ComponentModel.DataAnnotations.Schema;

namespace Collabrix.Models
{
    [Table("ChatFileAttachments")]
    public class ChatFileAttachment
    {
        [ForeignKey("ChatMessage")]
        public int MessageId { get; set; }

        [ForeignKey("FileAttachment")]
        public int AttachmentId { get; set; }
    }
}