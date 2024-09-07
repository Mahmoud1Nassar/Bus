using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bus.Data;
using Bus.Models;
using Bus.Repositories.Services;
using Microsoft.AspNetCore.Authorization;

namespace Bus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly RouteService _routeService;
        public RoutesController(RouteService routeService) { 
            _routeService = routeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Route>>> GetRoutes()
        {
           return await _routeService.GetRoutes();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Route>> GetRoute(int id)
        {
          return await _routeService.GetRoute(id);
        }

        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRoute(int id, Models.Route route)
        {
          return await _routeService.PutRoute(id, route);
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Models.Route>> PostRoute(Models.Route route)
        {
           return await _routeService.PostRoute(route);
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            return await _routeService.DeleteRoute(id);
        }
        
        private bool RouteExists(int id)
        {
          return _routeService.RouteExists(id);
        }
    }
}
