using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("Tasks")]
    public class Tasks
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [StringLength(150)]
        public string TaskName { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        [ForeignKey("ProjectTaskStage")]
        public int ProjectTaskStageId { get; set; }

        [ForeignKey("AssignedUser")]
        public int AssignedTo { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [ForeignKey("CreatedByUser")]
        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}