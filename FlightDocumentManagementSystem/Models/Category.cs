using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }
    }
}
