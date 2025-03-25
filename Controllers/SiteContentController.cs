// NSurePhysicsWebAPI/Controllers/SiteContentController.cs
using Microsoft.AspNetCore.Mvc;
using NSurePhysicsWebAPI.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using NSurePhysicsWebAPI.Settings;
using Microsoft.AspNetCore.Http.Extensions;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteContentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly PathsConfig _pathsConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SiteContentController(
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

        [HttpGet("GetAllSiteContents")]
        public async Task<ActionResult<IEnumerable<SiteContentDto>>> GetAllSiteContents()
        {
            try
            {
                var siteContents = await _context.GetAllSiteContentsAsync();
                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                foreach (var content in siteContents)
                {
                    if (!string.IsNullOrEmpty(content.ContentImageUrl))
                    {
                        content.ContentImageUrl = $"{baseUrl}/{_pathsConfig.Paths["SiteContentImages"]}/{content.ContentImageUrl}";
                    }
                }
                return Ok(siteContents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching site contents: {ex.Message}");
            }
        }

        [HttpPost("SaveSiteContent")]
        public async Task<IActionResult> SaveSiteContent([FromBody] SiteContentDto content)
        {
            try
            {
                if (content == null)
                    return BadRequest("Invalid site content data.");

                if (!string.IsNullOrEmpty(content.ContentImageUrl))
                {
                    content.ContentImageUrl = Path.GetFileName(content.ContentImageUrl); // Store only file name
                }

                await _context.SaveSiteContentAsync(content);
                return Ok(new { message = "Site content saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving site content: {ex.Message}");
            }
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var uploadsFolder = Path.Combine(_environment.WebRootPath, _pathsConfig.Paths["SiteContentImages"]);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var originalFileName = Path.GetFileName(file.FileName);
                var fileName = originalFileName;
                var filePath = Path.Combine(uploadsFolder, fileName);

                int counter = 1;
                while (System.IO.File.Exists(filePath))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                    var extension = Path.GetExtension(originalFileName);
                    fileName = $"{fileNameWithoutExtension} ({counter}){extension}";
                    filePath = Path.Combine(uploadsFolder, fileName);
                    counter++;
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { fileName = fileName }); // Return only file name
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }

        [HttpGet("GetAllPhotos")]
        public async Task<ActionResult<IEnumerable<PhotoGalleryDto>>> GetAllPhotos()
        {
            try
            {
                var photos = await _context.GetAllPhotosAsync();
                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                foreach (var photo in photos)
                {
                    if (!string.IsNullOrEmpty(photo.PhotoUrl))
                    {
                        photo.PhotoUrl = $"{baseUrl}/{_pathsConfig.Paths["SiteContentImages"]}/{photo.PhotoUrl}";
                    }
                }
                return Ok(photos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching photos: {ex.Message}");
            }
        }
    }
}