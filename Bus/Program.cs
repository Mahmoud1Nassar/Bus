using Bus.Data;
using Bus.Models;
using Bus.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace Bus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Dependency Injection for DbContext
            builder.Services.AddDbContext<BusDbContext>(options => options
                .UseSqlServer(builder.Configuration
                .GetConnectionString("DefaultConnection")));

            // Identity Service Configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BusDbContext>()
                .AddDefaultTokenProviders();

            // Add ApplicationUserService to DI container
            builder.Services.AddScoped<ApplicationUserService>();
            builder.Services.AddScoped<BusCollegeService>();
            builder.Services.AddScoped<RouteService>();
            builder.Services.AddScoped<ScheduledMaintenancesService>();

            // Add Controllers
            builder.Services.AddControllers();

            // JWT Configuration
            var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT:SecretKey"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,  // Disable Issuer validation
                    ValidateAudience = false,  // Disable Audience validation
                    ValidateLifetime = true,  // Ensure the token is not expired
                    ValidateIssuerSigningKey = true,  // Ensure the token is correctly signed
                    IssuerSigningKey = new SymmetricSecurityKey(key) // The key used to sign the token
                };
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Bus API",
                    Version = "v1",
                    Description = "Bus Management API for handling routes, buses, and more."
                });

                // Add JWT Bearer Authentication support
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string [] { }
        }
    });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bus API V1");
                    
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Ensure that authentication middleware is added
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
