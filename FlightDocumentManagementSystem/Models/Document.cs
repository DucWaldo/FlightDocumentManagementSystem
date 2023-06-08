using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Document")]
    public class Document
    {
        [Key]
        public Guid DocumentId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? PublicUrl { get; set; }
        public string? Version { get; set; }
        public bool Action { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public Guid FlightId { get; set; }
        [ForeignKey("FlightId")]
        public Flight? Flight { get; set; }

        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
    }
}
