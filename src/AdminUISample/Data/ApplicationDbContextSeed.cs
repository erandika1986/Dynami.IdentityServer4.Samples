
using FullFrameworkWithAdminUISample.Data;
using FullFrameworkWithAdminUISample.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace ASPNETIdentitySample.Data
{
    public class AuthIdentityContextSeed
    {
        private readonly ILogger<AuthIdentityContextSeed> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AuthIdentityContextSeed(
            ILogger<AuthIdentityContextSeed> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            this._logger = logger;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;

        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await SeedUsersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task SeedUsersAsync()
        {
            var adminUser = await _userManager.FindByNameAsync("Erandika");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "Erandika",
                    Email = "erandika1986@gmail.com",
                    EmailConfirmed = true

                };

                var adminRole = await _roleManager.FindByNameAsync("Administrator");

                var result = _userManager.CreateAsync(adminUser, "Pass123$").Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                if (adminRole != null)
                {
                    await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }

                result = _userManager.AddClaimsAsync(adminUser, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Admin"),
                            new Claim(JwtClaimTypes.GivenName, "Admin"),
                            new Claim(JwtClaimTypes.FamilyName, "Admin"),
                            new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
                            new Claim(JwtClaimTypes.Role, adminRole.Name),
                            new Claim("position", adminRole.Name),
                            new Claim("country", "USA"),
                            new Claim("subject", adminUser.Id),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                _logger.LogDebug("Admin");
            }
        }


    }
}
