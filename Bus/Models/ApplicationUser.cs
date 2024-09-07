using Microsoft.AspNetCore.Identity;

namespace Bus.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Gender { get; set; }
    }
}
