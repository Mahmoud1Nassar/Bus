namespace Bus.Models.DTO
{
    public class RouteDTO
    {
        public int RouteId { get; set; }
        public double Distance { get; set; } //Distance of the route
        public List<TimeSpan>? EstimatedTimeBetweenEachPoint { get; set; } //Estimated travel time
        public List<string>? StopPoints { get; set; } //List of stop points
        public int BusID { get; set; };
    }
}
