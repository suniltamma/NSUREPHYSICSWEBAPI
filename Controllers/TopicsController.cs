using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static TopicsDto;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TopicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllTopicsById")]
        public async Task<ActionResult<IEnumerable<TopicsDto>>> GetAllTopicsById([FromQuery] int? chapterID, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                // Call the service method with chapterID pagination parameters
                var topics = await _context.GetAllTopicsByIdAsync(chapterID, pageNumber, pageSize);

                // If no topics are returned, return a 404 Not Found
                if (topics == null || !topics.Any())
                {
                    return NotFound("No Topics found.");
                }

                // Return the results with an OK status


                return Ok(topics);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return StatusCode(500, $"Error fetching topics: {ex.Message}");
            }
        }


        [HttpPost("save-topics")]
        public async Task<IActionResult> SaveTopic([FromBody] SaveTopicDto topic)
        {
            try
            {
                if (topic == null)
                    return BadRequest("Invalid Topic data.");

               
                // 🔹 Execute stored procedure to save subject
                await _context.SaveTopicAsync(topic);
                return Ok(new { message = "Topic saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving topic: {ex.Message}");
            }
        }

        [HttpGet("GetAllTopicBySubjectID")]
        public async Task<ActionResult<IEnumerable<TopicsBySubID>>> GetAllTopicBySubjectID([FromQuery] int SubjectID)
        {
            try
            {
                // Call the service method with chapterID pagination parameters
                var topics = await _context.GetAllTopicBySubjectID(SubjectID);

                // If no topics are returned, return a 404 Not Found
                if (topics == null || !topics.Any())
                {
                    return NotFound("No Topics found.");
                }

                // Return the results with an OK status


                return Ok(topics);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return StatusCode(500, $"Error fetching topics: {ex.Message}");
            }
        }

    }
}
