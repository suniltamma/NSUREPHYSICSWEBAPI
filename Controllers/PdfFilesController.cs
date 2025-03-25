// NSurePhysicsWebAPI/Controllers/PdfFilesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using NSurePhysicsWebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using NSurePhysicsWebAPI.Settings;
using Microsoft.AspNetCore.Http.Extensions;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfFilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PdfFilesController>? _logger;
        private readonly PathsConfig _pathsConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PdfFilesController(
            ApplicationDbContext context,
            PathsConfig pathsConfig,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PdfFilesController>? logger = null)
        {
            _context = context;
            _pathsConfig = pathsConfig;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("GetSubjects")]
        public async Task<IActionResult> GetSubjects(int userId)
        {
            try
            {
                var attachments = await _context.GetTopicAttachmentsAsync(userId);
                var subjects = attachments
                    .GroupBy(a => new { a.SubjectId, a.SubjectName })
                    .Select(g => new SubjectDto
                    {
                        SubjectId = g.Key.SubjectId,
                        SubjectName = g.Key.SubjectName
                    })
                    .OrderBy(s => s.SubjectName)
                    .ToList();

                if (!subjects.Any())
                {
                    return NotFound("No subjects found for this user.");
                }

                return Ok(subjects);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error fetching subjects for user {UserId}", userId);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("GetChapters")]
        public async Task<IActionResult> GetChapters(int userId, int subjectId)
        {
            try
            {
                var attachments = await _context.GetTopicAttachmentsAsync(userId);
                var chapters = attachments
                    .Where(a => a.SubjectId == subjectId)
                    .GroupBy(a => new { a.ChapterId, a.ChapterName })
                    .Select(g => new ChapterDto
                    {
                        ChapterId = g.Key.ChapterId,
                        SubjectId = subjectId,
                        ChapterName = g.Key.ChapterName
                    })
                    .OrderBy(c => c.ChapterName)
                    .ToList();

                if (!chapters.Any())
                {
                    return NotFound("No chapters found for this subject.");
                }

                return Ok(chapters);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error fetching chapters for user {UserId}, subject {SubjectId}", userId, subjectId);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("GetTopicsWithAttachments")]
        public async Task<IActionResult> GetTopicsWithAttachments(int userId, int subjectId, int chapterId)
        {
            try
            {
                var attachments = await _context.GetTopicAttachmentsAsync(userId);
                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                var topics = attachments
                    .Where(a => a.ChapterId == chapterId && a.SubjectId == subjectId)
                    .Select(a => new TopicWithAttachmentDto
                    {
                        TopicsId = a.TopicsId,
                        TopicsName = a.TopicsName,
                        TopicAttachmentsId = a.TopicAttachmentsId,
                        TopicAttachmentFileName = $"{baseUrl}/{_pathsConfig.Paths["Pdfs"]}/{a.TopicAttachmentFileName}"
                    })
                    .OrderBy(t => t.TopicsName)
                    .ToList();

                if (!topics.Any())
                {
                    return NotFound("No topics or attachments found for this chapter.");
                }

                return Ok(topics);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error fetching topics for user {UserId}, chapter {ChapterId}", userId, chapterId);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}