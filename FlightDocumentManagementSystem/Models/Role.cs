using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }
        [Required]
        public string? Name { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }
    }
}
