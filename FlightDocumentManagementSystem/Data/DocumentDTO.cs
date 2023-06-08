namespace FlightDocumentManagementSystem.Data
{
    public class DocumentDTO
    {
        public IFormFile? File { get; set; }
        public Guid CategoryId { get; set; }
        public Guid FlightId { get; set; }
    }
}
