using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("TokenManager")]
    public class TokenManager
    {
        [Key]
        public Guid IdRefreshToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public Guid AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
    }
}
