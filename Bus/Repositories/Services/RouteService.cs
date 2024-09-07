using Bus.Data;
using Bus.Models.DTO;
using Bus.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus.Repositories.Services
{
    public class RouteService : IRoute
    {

        private readonly BusDbContext _context;

        public RouteService(BusDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return new NotFoundResult();
            }

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult<Models.Route>> GetRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);

            if (route == null)
            {
                return new NotFoundResult();
            }

            return route;
        }

        public async Task<ActionResult<IEnumerable<Models.Route>>> GetRoutes(RouteDTO routeDTO)
        {
            var filteredRoutes = await _context.Routes
           .Where(r =>
               (routeDTO.RouteId == null || r.RouteId == routeDTO.RouteId) &&                 // Filter by RouteId if provided
               (routeDTO.BusID == null || r.BusId == routeDTO.BusID) &&                       // Filter by BusId if provided
               (routeDTO.StopPoints == null || r.StopPoints.SequenceEqual(routeDTO.StopPoints)) && // Filter by StopPoints
               (routeDTO.EstimatedTimeBetweenEachPoint == null ||
                r.EstimatedTimeBetweenEachPoint.SequenceEqual(routeDTO.EstimatedTimeBetweenEachPoint)) // Filter by EstimatedTime
           )
           .ToListAsync();

            return new OkObjectResult(filteredRoutes);
        }

        public async Task<ActionResult<Models.Route>> PostRoute(RouteDTO routeDTO)
        {
            var route = new Models.Route
            {
                StopPoints = routeDTO.StopPoints,
                EstimatedTimeBetweenEachPoint = routeDTO.EstimatedTimeBetweenEachPoint,
                Distance = routeDTO.Distance,
                RouteId = routeDTO.RouteId,
                BusId = routeDTO.BusID
            };
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            return route;
        }

        public async Task<IActionResult> PutRoute(int id, Models.Route route)
        {
            if (id != route.RouteId)
            {
                return new BadRequestResult();
            }

            _context.Entry(route).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        public bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }
    }
}
