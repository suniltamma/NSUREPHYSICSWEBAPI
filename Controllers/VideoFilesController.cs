using Microsoft.AspNetCore.Mvc;
using NSurePhysicsWebAPI.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSurePhysicsWebAPI.Settings;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoFilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly PathsConfig _pathsConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VideoFilesController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            PathsConfig pathsConfig,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _environment = environment;
            _pathsConfig = pathsConfig;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetAllVideoFiles")]
        public async Task<ActionResult<IEnumerable<VideoFilesDto>>> GetAllVideoFiles([FromQuery] bool isSample = false)
        {
            try
            {
                var videoFiles = await _context.GetVideoFilesAsync(isSample);

                if (videoFiles == null || !videoFiles.Any())
                {
                    return Ok(new List<VideoFilesDto>()); // Return empty list if no data
                }

                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

                foreach (var video in videoFiles)
                {
                    if (!string.IsNullOrEmpty(video.FilePath))
                    {
                        video.FilePath = $"{baseUrl}/{_pathsConfig.Paths["VideoFiles"]}/{video.FilePath}";
                    }
                    // YoutubePath remains as is since it's already a full URL
                }

                return Ok(videoFiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching video files: {ex.Message} - Inner Exception: {ex.InnerException?.Message}");
            }
        }
    }
}