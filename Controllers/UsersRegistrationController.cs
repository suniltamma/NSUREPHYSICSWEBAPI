using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using NSurePhysicsWebAPI.DTOs;

[ApiController]
[Route("api/[controller]")]
public class UsersRegistrationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersRegistrationController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromForm] UserRegistrationDto userDto)
    {
        if (userDto == null)
            return BadRequest(new { message = "Invalid user data." });

        try
        {
            string? imagePath = null;

            // Handle image upload
            if (userDto.Image != null && userDto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/users");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"{Guid.NewGuid()}_{userDto.Image.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await userDto.Image.CopyToAsync(fileStream);
                }

                imagePath = $"assets/users/{uniqueFileName}";
            }

            userDto.ImagePath = imagePath;

            // Call the stored procedure and get the new UserId
            int newUserId = await _context.RegisterUserAsync(userDto);

            return Ok(new { message = "User registered successfully.", userId = newUserId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error registering user: {ex.Message}" });
        }
    }

    [HttpGet("classes/details")]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetClassesWithDetails()
    {
        try
        {
            var classes = await _context.GetAllClassesWithDetailsAsync();
            return Ok(classes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching classes: {ex.Message}");
        }
    }
}