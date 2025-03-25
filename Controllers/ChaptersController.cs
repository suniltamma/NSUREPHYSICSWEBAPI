using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChaptersController(ApplicationDbContext context)
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

        [HttpGet("syllabus")]
        public async Task<ActionResult<IEnumerable<string>>> GetSyllabus()
        {
            try
            {
                var syllabus = await _context.GetAllSyllabusAsync();
                return Ok(syllabus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching syllabus: {ex.Message}");
            }
        }



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


        [HttpGet("all-chapters-by-id")]
        public async Task<ActionResult<IEnumerable<ChapterDto>>> GetChaptersById([FromQuery] int classId, [FromQuery] int subjectID, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                // Call the service method with classid , subjectid, pagination parameters
                var chapters = await _context.GetAllChaptersIdAsync(classId, subjectID,pageNumber, pageSize);

                // If no chapters are returned, return a 404 Not Found
                if (chapters == null || !chapters.Any())
                {
                    return NotFound("No chapters found.");
                }

                // Return the results with an OK status


                return Ok(chapters);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return StatusCode(500, $"Error fetching chapters: {ex.Message}");
            }
        }


        [HttpGet("course-types")]
        public async Task<ActionResult<IEnumerable<CourseType>>> GetCourseTypes()
        {
            var courseTypes = await _context.CourseTypes.ToListAsync();
            return Ok(courseTypes);
        }


       

        [HttpGet("all-chapters")]
        public async Task<ActionResult<IEnumerable<ChapterDto>>> GetChapters()
        {
            try
            {
                var chapters = await _context.GetAllChaptersAsync();

                // If no chapters are returned, return a 404 Not Found
                if (chapters == null || !chapters.Any())
                {
                    return NotFound("No Chapters found.");
                }

                // Return the results with an OK status


                return Ok(chapters);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return StatusCode(500, $"Error fetching Chapters: {ex.Message}");
            }
        }



        [HttpPost("save-chapters")]
        public async Task<IActionResult> SaveChapter([FromBody] SaveChapterDto chapter)
        {
            try
            {
                if (chapter == null)
                    return BadRequest("Invalid chapter data.");

                // 🔹 Fetch all COURSE_TYPE entries from the database
                var allCourseTypes = await _context.CourseTypes.ToListAsync();
                Console.WriteLine("Fetched CourseType IDs: " + string.Join(", ", allCourseTypes.Select(ct => ct.CourseTypeId)));

                // 🔹 Check if the provided COURSE_TYPE_ID exists
                bool courseTypeExists = allCourseTypes.Any(c => c.CourseTypeId == chapter.CourseTypeId);

                if (!courseTypeExists)
                    return BadRequest($"Invalid COURSE_TYPE_ID ({chapter.CourseTypeId}). Please provide a valid one.");

                // 🔹 Execute stored procedure to save chapter
                await _context.SavechapterAsync(chapter);
                return Ok(new { message = "Chapter saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving chapter: {ex.Message}");
            }
        }


    }


}
