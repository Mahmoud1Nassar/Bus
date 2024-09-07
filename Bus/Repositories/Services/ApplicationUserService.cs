using Bus.Models;
using Bus.Models.DTO;
using Bus.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bus.Repositories.Services
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> RegisterUser(RegisterUserDTO registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Name,
                Email = registerDto.Email,
                Gender = registerDto.Gender,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult("User registered successfully.");
            }

            return new BadRequestObjectResult(result.Errors);
        }

        public async Task<IActionResult> LoginUser(LoginDTO loginModel)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            // Check if the user exists and verify password
            if (user == null)
            {
                return new UnauthorizedResult(); // User not found
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Fetch user roles from the database
                var userRoles = await _userManager.GetRolesAsync(user);

                // Create a list of claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID claim
                    new Claim(ClaimTypes.Email, user.Email),                  // User email claim
                    new Claim(ClaimTypes.Name, user.UserName)                 // User name claim
                };

                // Add the user's roles as claims
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role)); // Add each role the user has as a claim
                }

                // Create the token
                var key = Encoding.ASCII.GetBytes("gmvi1234354n5p34n5ipk2n3ip5npi3k123");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),              // Attach the claims (including roles) to the token
                    Expires = DateTime.UtcNow.AddMinutes(60),          // Token expiration time (60 minutes)
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new OkObjectResult(new { Token = tokenString });
            }

            // Incorrect credentials
            return new UnauthorizedResult();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new OkObjectResult("Logged out successfully");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByNameAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult("User not found.");
            }
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return new NoContentResult();
            }
            return new BadRequestObjectResult(result.Errors);
        }

        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            return user;
        }

        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return new OkObjectResult(users);
        }

        public async Task<IActionResult> AssignRole(string userEmail, AssignRoleDTO assignRoleDTO)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new NotFoundObjectResult($"There is no user with id {userEmail}");
            }
            var roleExists = await _roleManager.RoleExistsAsync(assignRoleDTO.Role);
            if (!roleExists)
            {
                return new BadRequestObjectResult($"There is no role {assignRoleDTO.Role}");
            }
            var result = await _userManager.AddToRoleAsync(user, assignRoleDTO.Role);
            if (result.Succeeded)
            {
                return new OkObjectResult($"Role {assignRoleDTO.Role} was assigned to user {user.UserName} successfully");
            }
            return new BadRequestObjectResult(result.Errors);
        }
    }
}
