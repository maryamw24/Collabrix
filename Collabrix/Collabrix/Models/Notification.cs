using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collabrix.Models
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("NotificationType")]
        public int NotificationTypeId { get; set; }

        public bool Read { get; set; } = false;

        [ForeignKey("Recipient")]
        public int RecipientId { get; set; }

        [ForeignKey("Sender")]
        public int? SenderId { get; set; }
    }
}
