using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("ProjectTaskStages")]

    public class ProjectTaskStage
    {
        [Key]
        public int StageId { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string StageName { get; set; }

        public string StageDescription { get; set; }

        [ForeignKey("CreatedByUser")]
        public int CreatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}