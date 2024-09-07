using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus.Models
{
    public class ScheduledMaintenance
    {
        [Key]
        public int MaintenanceId { get; set; }

        [ForeignKey("BusCollege")]
        public int BusId { get; set; }
        public BusCollege? BusCollege { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; } // Date when maintenance is scheduled

        [MaxLength(500)]
        public string? Description { get; set; } // Maintenance task description

        public string? Status { get; set; } // Status of the maintenance task (e.g., Scheduled, Completed, etc.)
    }
}
