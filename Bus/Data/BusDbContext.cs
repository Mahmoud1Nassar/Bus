using Microsoft.EntityFrameworkCore;
using Bus.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Identity;
namespace Bus.Data
{
    public class BusDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        // Constructor with dependency injection for IConfiguration

        public BusDbContext(DbContextOptions<BusDbContext> options,IConfiguration configuration)
            : base(options) { 
            _configuration = configuration;
        }
        public DbSet<BusCollege> Buses { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<ScheduledMaintenance> ScheduledMaintenance { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
        new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
        new IdentityRole { Id = "2", Name = "Driver", NormalizedName = "DRIVER" },
        new IdentityRole { Id = "3", Name = "Student", NormalizedName = "STUDENT" });

            // Define a constant ID for the admin user
            var adminId = "4";  // This ensures the user will always have this ID
            // Initialize PasswordHasher for ApplicationUser
            var hasher = new PasswordHasher<ApplicationUser>();
            // Seeding the admin user
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = adminId,  // Reference the constant ID here
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin@123"), 
                    SecurityStamp = string.Empty,
                    Gender = "Male"
                }
            );

            // Seeding the relationship between the admin user and the Admin role
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",  // The Admin role ID
                    UserId = adminId  // Reference the same constant ID for the admin user
                }
            );
        }



    }
   
}
