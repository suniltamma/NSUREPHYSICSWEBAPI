using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SubjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("classes")]
    public async Task<ActionResult<IEnumerable<string>>> GetClasses()
    {
        try
        {
            var classes = await _context.GetAllClassesdetailsAsync();
            return Ok(classes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching classes: {ex.Message}");
        }
    }

    //[HttpGet("all-subjects")]
    //public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
    //{
    //    try
    //    {
    //        var subjects = await _context.GetAllSubjectsAsync();
    //        return Ok(subjects);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Error fetching subjects: {ex.Message}");
    //    }
    //}

    [HttpGet("all-subjects")]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        try
        {
            // Call the service method with pagination parameters
            var subjects = await _context.GetAllSubjectsAsync(pageNumber, pageSize);

            // If no subjects are returned, return a 404 Not Found
            if (subjects == null || !subjects.Any())
            {
                return NotFound("No subjects found.");
            }

            // Return the results with an OK status
            

            return Ok(subjects);
        }
        catch (Exception ex)
        {
            // Return a 500 Internal Server Error with the exception message
            return StatusCode(500, $"Error fetching subjects: {ex.Message}");
        }
    }


    [HttpGet("course-types")]
    public async Task<ActionResult<IEnumerable<CourseType>>> GetCourseTypes()
    {
        var courseTypes = await _context.CourseTypes.ToListAsync();
        return Ok(courseTypes);
    }


    [HttpPost("save-subject")]
    public async Task<IActionResult> SaveSubject([FromBody] SaveSubjectDto subject)
    {
        try
        {
            if (subject == null)
                return BadRequest("Invalid subject data.");

            // 🔹 Fetch all COURSE_TYPE entries from the database
            var allCourseTypes = await _context.CourseTypes.ToListAsync();
            Console.WriteLine("Fetched CourseType IDs: " + string.Join(", ", allCourseTypes.Select(ct => ct.CourseTypeId)));

            // 🔹 Check if the provided COURSE_TYPE_ID exists
            bool courseTypeExists = allCourseTypes.Any(c => c.CourseTypeId == subject.CourseTypeId);

            if (!courseTypeExists)
                return BadRequest($"Invalid COURSE_TYPE_ID ({subject.CourseTypeId}). Please provide a valid one.");

            // 🔹 Execute stored procedure to save subject
            await _context.SaveSubjectAsync(subject);
            return Ok(new { message = "Subject saved successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error saving subject: {ex.Message}");
        }
    }


}
