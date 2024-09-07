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
    public class BusCollegesController : ControllerBase
    {
        private readonly BusCollegeService _context;

        public BusCollegesController(BusCollegeService context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusCollege>>> GetBuses()
        {
            return await _context.GetBuses();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<BusCollege>> GetBusById(int id)
        {
            return await _context.GetBusById(id);
        }

       
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBus(int id, BusCollege busCollege)
        {
           return await _context.UpdateBus(id, busCollege);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<BusCollege>> CreateBus(BusCollege busCollege)
        {
         return await _context.CreateBus(busCollege);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteBus(int id)
        {
           return await _context.DeleteBus(id);
        }
        
        private bool BusExists(int id)
        {
            return _context.BusExists(id);
        }
    }
}
