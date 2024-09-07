using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus.Models
{
    public class BusCollege
    {
        [Key]
        public int BusId { get; set; }
        public string? PlateNumber { get; set; }
        public int Capacity { get; set; }
        public string? Status { get; set; } 
        public string? DriverId { get; set; } // Reference to ApplicationUser (the driver)
        public ApplicationUser? Driver { get; set; } // Navigation property to Driver (optional)

    }
}
