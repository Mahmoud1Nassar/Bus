using Bus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bus.Repositories.Interfaces
{
    public interface IScheduledMaintenance
    {
        public Task<ActionResult<IEnumerable<ScheduledMaintenance>>> GetMaintenanceRecords();
        public Task<ActionResult<ScheduledMaintenance>> GetMaintenanceById(int id);
        public Task<ActionResult<ScheduledMaintenance>> CreateMaintenance(ScheduledMaintenance maintenance);
        public Task<IActionResult> UpdateMaintenance(int id, ScheduledMaintenance maintenance);
        public Task<IActionResult> DeleteMaintenance(int id);
        public bool MaintenanceExists(int id);
    }
}
