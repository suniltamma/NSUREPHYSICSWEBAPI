// NSurePhysicsWebAPI/Controllers/PackageController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NSurePhysicsWebAPI.DTOs;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageStudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PackageStudentController>? _logger;

        public PackageStudentController(ApplicationDbContext context, ILogger<PackageStudentController>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetPackages")]
        public async Task<IActionResult> GetPackages(int userId)
        {
            try
            {
                var packages = await _context.GetPackagesAsync(userId);
                if (!packages.Any())
                {
                    return NotFound("No packages found.");
                }

                return Ok(packages);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error fetching packages for user {UserId}", userId);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}





