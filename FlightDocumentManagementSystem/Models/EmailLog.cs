using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("EmailLog")]
    public class EmailLog
    {
        [Key]
        public Guid EmailLogId { get; set; }
        public bool Status { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime TimeUpdate { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
    }
}
