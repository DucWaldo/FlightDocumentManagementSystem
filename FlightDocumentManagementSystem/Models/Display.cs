using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Display")]
    public class Display
    {
        [Key]
        public Guid DisplayId { get; set; }
        public string? LogoUrl { get; set; }
        public bool CaptchaStatus { get; set; }

        public DateTime TimeCreate { get; set; }
        public DateTime TimeUpdate { get; set; }
    }
}
