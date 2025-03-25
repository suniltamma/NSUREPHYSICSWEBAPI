using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class DatabaseTestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DatabaseTestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("test-db")]
    public async Task<IActionResult> TestDatabaseConnection()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            return canConnect ? Ok("✅ Database Connection Successful!") : BadRequest("❌ Failed to connect to the database.");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "❌ Error connecting to DB", details = ex.Message });
        }
    }

}
