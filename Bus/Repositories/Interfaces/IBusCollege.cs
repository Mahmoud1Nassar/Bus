using Bus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bus.Repositories.Interfaces
{
    public interface IBusCollege
    {
        public Task<ActionResult<IEnumerable<BusCollege>>> GetBuses();
        public Task<ActionResult<BusCollege>> GetBusById(int id);
        public Task<ActionResult<BusCollege>> CreateBus(BusCollege bus);
        public Task<IActionResult> UpdateBus(int id, BusCollege bus);
        public  Task<IActionResult> DeleteBus(int id);
        public bool BusExists(int id);
    }
}
