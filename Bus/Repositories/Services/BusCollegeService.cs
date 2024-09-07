using Bus.Data;
using Bus.Models;
using Bus.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bus.Repositories.Services
{
    public class BusCollegeService : IBusCollege
    {
        private readonly BusDbContext _context;

        public BusCollegeService(BusDbContext context)
        {
            _context = context;
        }

        // Fetch all buses, including driver details
        public async Task<ActionResult<IEnumerable<BusCollege>>> GetBuses()
        {
            return await _context.Buses.Include(b => b.Driver).ToListAsync();
        }

        // Fetch a specific bus by ID
        public async Task<ActionResult<BusCollege>> GetBusById(int id)
        {
            var bus = await _context.Buses.Include(b => b.Driver).FirstOrDefaultAsync(b => b.BusId == id);
            if (bus == null)
            {
                return new NotFoundResult();
            }
            return bus;
        }

        // Create a new bus
        public async Task<ActionResult<BusCollege>> CreateBus(BusCollege bus)
        {
            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetBusById), "Bus", new { id = bus.BusId }, bus);
        }

        // Update an existing bus
        public async Task<IActionResult> UpdateBus(int id, BusCollege bus)
        {
            if (id != bus.BusId)
            {
                return new BadRequestResult();
            }

            _context.Entry(bus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusExists(id))
                {
                    return new NotFoundResult();
                }
                throw;
            }

            return new NoContentResult();
        }

        // Delete a bus
        public async Task<IActionResult> DeleteBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return new NotFoundResult();
            }

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public bool BusExists(int id)
        {
            return _context.Buses.Any(e => e.BusId == id);
        }
    }
}
