using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collabrix.Models
{
    [Table("Lookups")]
    public class Lookup
    {
        [Key]
        public int LookupId { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}