using System.ComponentModel.DataAnnotations.Schema;

namespace Collabrix.Models
{
    [Table("UserProjects")]
    public class UserProject
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("RoleLookup")]
        public int Role { get; set; }
    }
}