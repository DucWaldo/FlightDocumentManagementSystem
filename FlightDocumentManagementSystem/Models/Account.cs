using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool Status { get; set; }

        public DateTime TimeCreate { get; set; }
        public DateTime TimeUpdate { get; set; }

        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
    }
}
