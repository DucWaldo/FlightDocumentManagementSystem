using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateTime DateStart { get; set; }
        public bool Status { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }

        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
    }
}
