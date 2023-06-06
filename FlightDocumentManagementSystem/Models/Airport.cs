using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Airport")]
    public class Airport
    {
        [Key]
        public Guid AirportId { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? IATACode { get; set; }
        public string? ICAOCode { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Timezone { get; set; }
        public string? Facility { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }

        public DateTime TimeCreate { get; set; }
        public DateTime TimeUpdate { get; set; }
    }
}
