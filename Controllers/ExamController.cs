using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Xml.Linq;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;


namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }



        //[HttpGet("GetAllExams")]
        //public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllExams([FromQuery] int UserID)
        //{
        //    try
        //    {

        //        var exams = await _context.GetAllExamsByIdAsync(UserID);


        //        if (exams == null || !exams.Any())
        //        {
        //            return NotFound("No Exams found.");
        //        }

        //        // Return the results with an OK status


        //        return Ok(exams);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return a 500 Internal Server Error with the exception message
        //        return StatusCode(500, $"Error fetching Exams: {ex.Message}");
        //    }
        //}

        [HttpGet("GetAllExams")]
        public async Task<ActionResult> GetAllExams([FromQuery] int UserID)
        {
            try
            {
                // Call the method to get exams and exam subjects
                var (exams, examSubjects) = await _context.GetAllExamsByIdAsync(UserID);

                // Check if exams are found
                if (exams == null || !exams.Any())
                {
                    return NotFound("No Exams found.");
                }

                // Return both exams and exam subjects in the response
                var result = new
                {
                    Exams = exams,
                    ExamSubjects = examSubjects
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return StatusCode(500, $"Error fetching Exams: {ex.Message}");
            }
        }

        [HttpPost("save-exam")]
        public async Task<IActionResult> SaveExam([FromBody] SaveExamDto examDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var examSubjects = new List< ExamSubjectdetailsDto > ();
                foreach (var subject in examDto.exam_Subject)
                {
                    var deserializedSubject = JsonSerializer.Deserialize < ExamSubjectdetailsDto > (subject);
                    if (deserializedSubject != null)
                    {
                        examSubjects.Add(deserializedSubject);
                    }
                }

                var examSubjectsXml = GenerateExamSubjectsXml(examSubjects);

                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_SAVE_EXAM @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13",
                    examDto.examId, examDto.examName, examDto.Syllabus_Id, examDto.examType, examDto.ExamDate,
                    examDto.Duration, examDto.CourseTypeId, examDto.UserId, examDto.createdDate, examDto.modifiedDate,
                    examDto.IsActive, examDto.ClassId, examDto.Exam_Subject_Id, examSubjectsXml
                );

                return Ok(new { message = "Exam saved successfully", result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

       

        

        private string GenerateExamSubjectsXml(List<ExamSubjectdetailsDto> examSubjects)
        {
            var xml = new XElement("EXAM_SUBJECT",
                examSubjects.Select(subject => new XElement("RESULT",
                    new XAttribute("CHAPTER_ID", subject.CHAPTER_ID),
                    new XAttribute("SUBJECT_ID", subject.SUBJECT_ID),
                    new XAttribute("TOPIC_ID", subject.TOPIC_ID),
                    new XAttribute("NO_OF_QUESTIONS", subject.NO_OF_QUESTIONS),
                    new XAttribute("CREATED_BY", subject.CREATED_BY),
                    new XAttribute("MODIFIED_BY", subject.MODIFIED_BY),
                    new XAttribute("IS_ACTIVE", subject.IS_ACTIVE ? 1 : 0)
                ))
            );
            return xml.ToString();
        }




    }
}
