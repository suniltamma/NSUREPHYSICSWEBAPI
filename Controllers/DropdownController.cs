using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DropdownController : ControllerBase
{
    [HttpGet("classes")]
    public IActionResult GetClasses()
    {
        return Ok(new List<string> { "Class 1", "Class 2", "Class 3" });
    }

    [HttpGet("boards")]
    public IActionResult GetBoards()
    {
        return Ok(new List<string> { "CBSE", "ICSE", "State Board" });
    }
}
