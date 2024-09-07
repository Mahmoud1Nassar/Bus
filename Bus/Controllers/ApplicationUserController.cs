using Bus.Models;
using Bus.Models.DTO;
using Bus.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {

        private readonly ApplicationUserService _applicationUserService;
        

        public ApplicationUserController(ApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
           return await _applicationUserService.GetUsers();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            return await _applicationUserService.GetUser(id);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO registerDto)
        {
            return await _applicationUserService.RegisterUser(registerDto);
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO loginModel)
        {
            return await _applicationUserService.LoginUser(loginModel);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            return await _applicationUserService.Logout();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return await _applicationUserService.DeleteUser(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Assign Role")]
        public async Task<IActionResult> AssignRole(string userEmail, AssignRoleDTO assignRoleDTO)
        {
            return await _applicationUserService.AssignRole(userEmail, assignRoleDTO);
        }
    }
}
