using Bus.Models;
using Bus.Models.DTO;
using Microsoft.AspNetCore.Mvc;


namespace Bus.Repositories.Interfaces
{
    public interface IApplicationUser
    {
        public Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO registerDto);
        public Task<IActionResult> LoginUser([FromBody] LoginDTO loginModel);
        public Task<IActionResult> Logout();
        public Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers();
        public Task<ActionResult<ApplicationUser>> GetUser(string id);
        public Task<IActionResult> DeleteUser(string id);
        public Task<IActionResult> AssignRole(string UserID, AssignRoleDTO assignRoleDTO);
       
    }
}
