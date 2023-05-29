using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Flight")]
    public class Flight
    {
        [Key]
        public Guid FlightId { get; set; }
        [Required]
        public string? FlightNo { get; set; }
        public DateTime? Date { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }
    }
}
