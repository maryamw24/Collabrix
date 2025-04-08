using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("TaskComments")]
    public class TaskComment
    {
        [Key]
        public int CommentId { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        public string CommentText { get; set; }

        [ForeignKey("CommentedUser")]
        public int CommentedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}