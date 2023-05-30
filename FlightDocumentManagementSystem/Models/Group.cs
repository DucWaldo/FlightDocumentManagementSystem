using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Group")]
    public class Group
    {
        [Key]
        public Guid GroupId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Note { get; set; }
        [Required]
        public string? Creator { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }
    }
}
