using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Xml.Linq;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting;
using NSurePhysicsWebAPI.DTOs; // For IWebHostEnvironment

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ScheduleController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment; // Inject IWebHostEnvironment
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserPackageSchedule(int userId)
    {
        try
        {
            var schedule = await _context.GetUserPackageScheduleAsync(userId);
            return Ok(schedule);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error fetching schedule: {ex.Message}" });
        }
    }

    [HttpGet("user/{userId}/schedule/{scheduleId}")]
    public async Task<IActionResult> GetUserPackageScheduleDetails(int userId, int scheduleId)
    {
        try
        {
            var (schedule, attachments) = await _context.GetUserPackageScheduleDetailsAsync(userId, scheduleId);
            return schedule == null
                ? NotFound(new { Message = $"No schedule found for USER_ID={userId} and SCHEDULE_ID={scheduleId}" })
                : Ok(new { Schedule = schedule, Attachments = attachments });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Error fetching schedule details: {ex.Message}" });
        }
    }

    [HttpGet("file/{fileName}")]
    public IActionResult GetScheduleFile(string fileName)
    {
        try
        {
            var filePath = Path.Combine(_environment.WebRootPath, "pdfs", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { Message = $"File '{fileName}' not found in wwwroot/pdfs." });
            }

            return PhysicalFile(filePath, "application/pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Error serving file: {ex.Message}" });
        }
    }

    [HttpGet("GetAllScheduleByPackageID")]
    public async Task<ActionResult<IEnumerable<ScheduleDetailsbyIdDto>>> GetAllScheduleByPackageID(int packageID)
    {
        try
        {
            var packages = await _context.GetAllScheduleByPackageIDAsync(packageID);
            return Ok(packages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching packages: {ex.Message}");
        }
    }



    [HttpPost("save-schedule")]
    public async Task<IActionResult> SaveSchedule([FromBody] SaveScheduleDto scheduleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var schedules = new List<SaveScheduleDetailsDto>();
            foreach (var schedule in scheduleDto.Schedules)
            {
                var deserializedSchedule = JsonSerializer.Deserialize<SaveScheduleDetailsDto>(schedule);
                if (deserializedSchedule != null)
                {
                    schedules.Add(deserializedSchedule);
                }
            }

            var packageSchedulesXml = GenerateScheduleXml(schedules);

            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_SAVE_PACKAGE_SCHEDULE @p0, @p1",
                packageSchedulesXml, scheduleDto.UserId
            );

            return Ok(new { message = "Schedule saved successfully", result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    private string GenerateScheduleXml(List<SaveScheduleDetailsDto> schedules)
    {
        var xml = new XElement("PACKAGE_SCHEDULE",
            schedules.Select(schedule => new XElement("SCHEDULE",
                new XAttribute("SCHEDULE_NAME", schedule.ScheduleName ?? string.Empty),
                new XAttribute("PACKAGE_ID", schedule.PackageId),
                new XAttribute("SCHEDULE_DAY_NO", schedule.ScheduleDayNo),
                new XAttribute("SCHEDULE_DATE", schedule.ScheduleDate.ToString("yyyy-MM-dd")),
                new XAttribute("START_TIME", schedule.StartTime.ToString("hh\\:mm\\:ss\\.fffffff")),
                new XAttribute("END_TIME", schedule.EndTime.ToString("hh\\:mm\\:ss\\.fffffff")),
                new XAttribute("DURATION", schedule.Duration),
                new XAttribute("TOPICS_ID", schedule.TopicsId),
                new XAttribute("INSTRUCTOR_ID", schedule.InstructorId),
                new XAttribute("LIVE_CLASS_URL", schedule.LiveClassUrl ?? string.Empty),
                new XAttribute("PACKAGE_SUBJECT_ID", schedule.PackageSubjectId)
            ))
        );

        return xml.ToString();
    }




    [HttpPost("UpdateScheduleBYID")]
    public async Task<IActionResult> UpdateScheduleBYID([FromBody] UpdateScheduleDetailsDto schedule)
    {
        try
        {
            if (schedule == null)
                return BadRequest("Invalid schedule data.");

            // Execute stored procedure to save schedule

            await _context.UpdateScheduleByScheIDAsync(schedule);

            return Ok(new { message = "schedule saved successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error saving schedule: {ex.Message}");
        }
    }
}