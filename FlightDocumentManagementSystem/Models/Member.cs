using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Member")]
    public class Member
    {
        [Key]
        public Guid MemberId { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }

        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }

        public Guid GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group? Group { get; set; }
    }
}
