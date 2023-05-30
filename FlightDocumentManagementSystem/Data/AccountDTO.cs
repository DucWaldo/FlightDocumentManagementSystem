namespace FlightDocumentManagementSystem.Data
{
    public class AccountDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Guid RoleId { get; set; }
    }
}
