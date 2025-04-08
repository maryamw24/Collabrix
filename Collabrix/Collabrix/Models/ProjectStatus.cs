using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("ProjectStatuses")]
    public class ProjectStatus
    {
        [Key]
        public int ProjectStatusId { get; set; }

        [Required]
        [StringLength(100)]
        public string StatusName { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
    }
}
