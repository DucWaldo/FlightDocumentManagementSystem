namespace FlightDocumentManagementSystem.Data
{
    public class UserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Birthday { get; set; }
        public bool Gender { get; set; } // false = Female, true = Male
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DateStart { get; set; }
        public Guid RoleId { get; set; }
    }
}
