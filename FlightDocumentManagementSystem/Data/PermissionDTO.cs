namespace FlightDocumentManagementSystem.Data
{
    public class PermissionDTO
    {
        public string? Function { get; set; }
        public Guid GroupId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
