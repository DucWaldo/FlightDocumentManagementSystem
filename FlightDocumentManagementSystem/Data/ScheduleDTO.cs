namespace FlightDocumentManagementSystem.Data
{
    public class ScheduleDTO
    {
        public int Point { get; set; }
        public Guid AirportId { get; set; }
        public Guid FlightId { get; set; }
        public Guid AircraftId { get; set; }
    }
}
