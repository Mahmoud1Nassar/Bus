using Bus.Data;
using Bus.Models;
using Bus.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus.Repositories.Services
{
    public class ScheduledMaintenancesService : IScheduledMaintenance
    {
        private readonly BusDbContext _context;

        public ScheduledMaintenancesService(BusDbContext context)
        {
            _context = context;
        }

        // Fetch all scheduled maintenance records
        public async Task<ActionResult<IEnumerable<ScheduledMaintenance>>> GetMaintenanceRecords()
        {
            return await _context.ScheduledMaintenance.Include(m => m.BusCollege).ToListAsync();
        }

        // Fetch a specific maintenance record by ID
        public async Task<ActionResult<ScheduledMaintenance>> GetMaintenanceById(int id)
        {
            var maintenance = await _context.ScheduledMaintenance.Include(m => m.BusCollege).FirstOrDefaultAsync(m => m.MaintenanceId == id);
            if (maintenance == null)
            {
                return new NotFoundResult();
            }
            return maintenance;
        }

        // Create a new maintenance record
        public async Task<ActionResult<ScheduledMaintenance>> CreateMaintenance(ScheduledMaintenance maintenance)
        {
            _context.ScheduledMaintenance.Add(maintenance);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetMaintenanceById), "ShecduleMaintenance", new { id = maintenance.MaintenanceId }, maintenance);
        }

        // Update an existing maintenance record
        public async Task<IActionResult> UpdateMaintenance(int id, ScheduledMaintenance maintenance)
        {
            if (id != maintenance.MaintenanceId)
            {
                return new BadRequestResult();
            }

            _context.Entry(maintenance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(id))
                {
                    return new NotFoundResult();
                }
                throw;
            }

            return new NoContentResult();
        }

        // Delete a maintenance record
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            var maintenance = await _context.ScheduledMaintenance.FindAsync(id);
            if (maintenance == null)
            {
                return new NotFoundResult();
            }

            _context.ScheduledMaintenance.Remove(maintenance);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public bool MaintenanceExists(int id)
        {
            return _context.ScheduledMaintenance.Any(m => m.MaintenanceId == id);
        }
    }
}

