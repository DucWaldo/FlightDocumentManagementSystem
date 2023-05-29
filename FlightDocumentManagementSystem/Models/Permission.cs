using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocumentManagementSystem.Models
{
    [Table("Permission")]
    public class Permission
    {
        [Key]
        public Guid PermissionId { get; set; }
        [Required]
        public string? Function { get; set; }

        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeUpdate { get; set; }

        public Guid GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document? Document { get; set; }
    }
}
