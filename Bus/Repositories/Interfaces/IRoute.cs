using Bus.Models;
using Bus.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Bus.Repositories.Interfaces
{
    public interface IRoute
    {
        public Task<ActionResult<IEnumerable<Models.Route>>> GetRoutes(RouteDTO routeDTO);
        public Task<ActionResult<Models.Route>> GetRoute(int id);
        public Task<IActionResult> PutRoute(int id, Models.Route route);
        public  Task<ActionResult<Models.Route>> PostRoute(RouteDTO routeDTO);
        public  Task<IActionResult> DeleteRoute(int id);
        public bool RouteExists(int id);

    }
}
