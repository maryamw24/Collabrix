using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    public class ChatMessage
    {
        [Key]
        public int MessageId { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Required]
        public string MessageText { get; set; }

        [ForeignKey("SentByUser")]
        public int SentBy { get; set; }

        public DateTime SentAt { get; set; }
    }
}
