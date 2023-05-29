using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Aircraft")]
    public class Aircraft
    {
        [Key]
        public Guid AircraftId { get; set; }
        [Required]
        public string? Serial { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int YearOfManufacure { get; set; }
        public int Status { get; set; }
    }
}
