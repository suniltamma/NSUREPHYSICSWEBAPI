using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using static TopicsDto;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public QuestionController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet("GetAllQuestions")]
        public async Task<ActionResult<IEnumerable<QuestionsDto>>> GetAllQuestions()
        {
            try
            {
                var questions = await _context.GetAllQuestionsAsync();
                return Ok(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching questions: {ex.Message}");
            }
        }

        [HttpPost("SaveQuestions")]
        public async Task<IActionResult> SaveQuestions([FromBody] SaveQuestionDto questions)
        {
            try
            {
                if (questions == null)
                    return BadRequest("Invalid Question data.");

                await _context.SaveQuestionsAsync(questions);
                return Ok(new { message = "Question saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving question: {ex.Message}");
            }
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "question-images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Use the original file name
                var originalFileName = Path.GetFileName(file.FileName); // e.g., "myimage.png"
                var fileName = originalFileName;    
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Check for file name conflicts and append a number if necessary
                int counter = 1;
                while (System.IO.File.Exists(filePath))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName); // e.g., "myimage"
                    var extension = Path.GetExtension(originalFileName); // e.g., ".png"
                    fileName = $"{fileNameWithoutExtension} ({counter}){extension}"; // e.g., "myimage (1).png"
                    filePath = Path.Combine(uploadsFolder, fileName);
                    counter++;
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var imageUrl = $"{Request.Scheme}://{Request.Host}/question-images/{fileName}";
                return Ok(new { url = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }
    }
}