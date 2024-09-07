using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public double Distance { get; set; } //Distance of the route
        public List<TimeSpan>? EstimatedTimeBetweenEachPoint { get; set; } //Estimated travel time
        public List<string>? StopPoints { get; set; } //List of stop points

        [ForeignKey("BusCollege")]
        public int BusId { get; set; }
        public BusCollege? busCollege { get; set; }

    }
}
