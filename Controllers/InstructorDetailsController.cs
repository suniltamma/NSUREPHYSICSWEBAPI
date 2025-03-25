using Microsoft.AspNetCore.Mvc;
using NSurePhysicsWebAPI.DTOs;
using System.Threading.Tasks;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InstructorDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetInstructorDetails")]
        public async Task<ActionResult<IEnumerable<InstructorDetailsDto>>> GetInstructorDetails(
            [FromQuery] int? instructorId = null,
            [FromQuery] int? userId = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var instructorDetails = await _context.GetInstructorDetailsAsync(instructorId, userId, isActive);
                return Ok(instructorDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching instructor details: {ex.Message}");
            }
        }
    }
}