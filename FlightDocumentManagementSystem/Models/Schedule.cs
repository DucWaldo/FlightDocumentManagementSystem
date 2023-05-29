using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [Key]
        public Guid ScheduleId { get; set; }
        public int Point { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }

        public Guid AirportId { get; set; }
        [ForeignKey("AirportId")]
        public Airport? Airport { get; set; }

        public Guid FlightId { get; set; }
        [ForeignKey("FlightId")]
        public Flight? Flight { get; set; }
        public Guid AircraftId { get; set; }
        [ForeignKey("AircraftId")]
        public Aircraft? Aircraft { get; set; }
    }
}
