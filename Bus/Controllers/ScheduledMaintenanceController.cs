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

namespace Bus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledMaintenanceController : ControllerBase
    {
        private readonly ScheduledMaintenancesService _context;

        public ScheduledMaintenanceController(ScheduledMaintenancesService context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledMaintenance>>> GetMaintenanceRecords() { 
            return await _context.GetMaintenanceRecords();
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduledMaintenance>> GetMaintenanceById(int id)
        {
            return await _context.GetMaintenanceById(id);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaintenance(int id, ScheduledMaintenance maintenance)
        {
           return await _context.UpdateMaintenance(id, maintenance);
        }

       
        [HttpPost]
        public async Task<ActionResult<ScheduledMaintenance>> CreateMaintenance(ScheduledMaintenance maintenance)
        {
           return await _context.CreateMaintenance(maintenance);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
           return await _context.DeleteMaintenance(id);
        }
        
        private bool MaintenanceExists(int id)
        {
          return _context.MaintenanceExists(id);
        }
    }
}
