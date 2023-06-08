using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Signature")]
    public class Signature
    {
        [Key]
        public Guid SignatureId { get; set; }
        [Required]
        public string? Url { get; set; }
        public string? PublicUrl { get; set; }

        public DateTime TimeCreate { get; set; }
        public DateTime TimeUpdate { get; set; }

        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document? Document { get; set; }
    }
}
