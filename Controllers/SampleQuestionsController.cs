using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSurePhysicsWebAPI.DTOs;
using System.Data;
using NSurePhysicsWebAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Text;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleQuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SampleQuestionsController> _logger;

        public SampleQuestionsController(ApplicationDbContext context, ILogger<SampleQuestionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetTestQuestions")]
        public async Task<IActionResult> GetTestQuestions(int examId)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_GET_RANDOM_QUESTION";

                        var examIdParam = new SqlParameter("@EXAM_ID", SqlDbType.Int) { Value = examId };
                        command.Parameters.Add(examIdParam);

                        var questions = new List<object>();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                questions.Add(new
                                {
                                    questionId = reader.IsDBNull(reader.GetOrdinal("QUESTION_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QUESTION_ID")),
                                    repeatedQuestionYear = reader.IsDBNull(reader.GetOrdinal("REPEATED_QUESTION_YEAR")) ? null : reader.GetString(reader.GetOrdinal("REPEATED_QUESTION_YEAR")),
                                    questionName = reader.IsDBNull(reader.GetOrdinal("QUESTION_NAME")) ? null : reader.GetString(reader.GetOrdinal("QUESTION_NAME")),
                                    questionAnswer = reader.IsDBNull(reader.GetOrdinal("QUESTION_ANSWER")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QUESTION_ANSWER")),
                                    solutionAnalysis = reader.IsDBNull(reader.GetOrdinal("SOLUTION_ANALYSIS")) ? null : reader.GetString(reader.GetOrdinal("SOLUTION_ANALYSIS")),
                                    option1 = reader.IsDBNull(reader.GetOrdinal("OPTION_1")) ? null : reader.GetString(reader.GetOrdinal("OPTION_1")),
                                    option2 = reader.IsDBNull(reader.GetOrdinal("OPTION_2")) ? null : reader.GetString(reader.GetOrdinal("OPTION_2")),
                                    option3 = reader.IsDBNull(reader.GetOrdinal("OPTION_3")) ? null : reader.GetString(reader.GetOrdinal("OPTION_3")),
                                    option4 = reader.IsDBNull(reader.GetOrdinal("OPTION_4")) ? null : reader.GetString(reader.GetOrdinal("OPTION_4")),
                                    topicsId = reader.IsDBNull(reader.GetOrdinal("TOPICS_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TOPICS_ID")),
                                    questionTypicalLevelId = reader.IsDBNull(reader.GetOrdinal("QUESTION_TYPICAL_LEVEL_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QUESTION_TYPICAL_LEVEL_ID")),
                                    examSubjectId = reader.IsDBNull(reader.GetOrdinal("EXAM_SUBJECT_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("EXAM_SUBJECT_ID")) // Added
                                });
                            }
                        }

                        if (!questions.Any())
                        {
                            return Ok(new { message = "No questions found for this exam.", questions = new List<object>() });
                        }

                        return Ok(questions);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random questions for exam {ExamId}", examId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("SubmitExam")]
        public async Task<IActionResult> SubmitExam([FromBody] List<ExamResultDto> studentAnswers)
        {
            if (studentAnswers == null || !studentAnswers.Any())
            {
                return BadRequest("No answers submitted.");
            }

            try
            {
                var userId = studentAnswers.First().UserId;
                if (studentAnswers.Any(a => a.UserId != userId))
                {
                    return BadRequest("All answers must belong to the same user.");
                }

                StringBuilder xmlBuilder = new StringBuilder();
                xmlBuilder.Append("<EXAM_RESULT>");
                foreach (var answer in studentAnswers)
                {
                    xmlBuilder.AppendFormat(
                        "<RESULT MARKED_ANSWERS=\"{0}\" QUESTION_ID=\"{1}\" EXAM_SUBJECT_ID=\"{2}\" EXAM_ID=\"{3}\"/>",
                        answer.MarkedAnswer,
                        answer.ExamQuestionId,
                        answer.ExamSubjectId,
                        answer.ExamId
                    );
                }
                xmlBuilder.Append("</EXAM_RESULT>");

                string xmlString = xmlBuilder.ToString();
                _logger.LogInformation("Generated XML: {Xml}", xmlString);

                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_SAVE_EXAM_RESULT";

                        // Add XML parameter
                        command.Parameters.Add(new SqlParameter("@EXAM_RESULT", SqlDbType.Xml) { Value = xmlString });

                        // Add USERS_ID parameter
                        command.Parameters.Add(new SqlParameter("@USERS_ID", SqlDbType.Int) { Value = userId });

                        // Add ATTEMPT_NUMBER as an OUTPUT parameter
                        var attemptNumberParam = new SqlParameter("@ATTEMPT_NUMBER", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(attemptNumberParam);

                        // Execute the stored procedure and capture returned data
                        var examResultIds = new List<int>();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            // The SP doesn't return rows by default now, so this might be empty unless you uncomment SELECT * FROM @RETURN
                            while (await reader.ReadAsync())
                            {
                                int questionId = reader.GetInt32(reader.GetOrdinal("QUESTION_ID"));
                                examResultIds.Add(questionId);
                            }
                        }

                        // Get the output value of ATTEMPT_NUMBER
                        int attemptNumber = attemptNumberParam.Value != DBNull.Value ? (int)attemptNumberParam.Value : 0;

                        return Ok(new
                        {
                            message = "Exam results saved successfully.",
                            examResultIds = examResultIds, // Might be empty unless SP returns data
                            attemptNumber = attemptNumber
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving exam results");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("GetUserExams")]
        public IActionResult GetUserExams(int userId)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_GET_USER_EXAMS";

                        var userIdParam = command.CreateParameter();
                        userIdParam.ParameterName = "@USERS_ID";
                        userIdParam.Value = userId;
                        command.Parameters.Add(userIdParam);

                        var exams = new List<object>();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                exams.Add(new
                                {
                                    examId = reader.IsDBNull(reader.GetOrdinal("EXAM_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("EXAM_ID")),
                                    examName = reader.IsDBNull(reader.GetOrdinal("EXAM_NAME")) ? null : reader.GetString(reader.GetOrdinal("EXAM_NAME")),
                                    examType = reader.IsDBNull(reader.GetOrdinal("EXAM_TYPE")) ? null : reader.GetString(reader.GetOrdinal("EXAM_TYPE")),
                                    duration = reader.IsDBNull(reader.GetOrdinal("DURATION")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DURATION")),
                                    syllabusId = reader.IsDBNull(reader.GetOrdinal("SYLLABUS_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SYLLABUS_ID")),
                                    syllabusName = reader.IsDBNull(reader.GetOrdinal("SYLLABUS_NAME")) ? null : reader.GetString(reader.GetOrdinal("SYLLABUS_NAME")),
                                    courseTypeId = reader.IsDBNull(reader.GetOrdinal("COURSE_TYPE_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("COURSE_TYPE_ID")),
                                    courseTypeName = reader.IsDBNull(reader.GetOrdinal("COURSE_TYPE_NAME")) ? null : reader.GetString(reader.GetOrdinal("COURSE_TYPE_NAME")),
                                    classId = reader.IsDBNull(reader.GetOrdinal("CLASS_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CLASS_ID")),
                                    className = reader.IsDBNull(reader.GetOrdinal("CLASS_NAME")) ? null : reader.GetString(reader.GetOrdinal("CLASS_NAME"))
                                });
                            }
                        }

                        if (!exams.Any())
                        {
                            return Ok(new { message = "No exams found for this user.", exams = new List<object>() });
                        }

                        return Ok(exams);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching exams for user {UserId}", userId);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("GetSampleTestParams")]
        public IActionResult GetSampleTestParams(int userId, int? examId = null, int? examSubjectId = null)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    connection.Open();
                    int? examIdFromResult = null;
                    string examName = null;
                    int? examSubjectIdFromResult = null;
                    int duration = 0;
                    int classIdFromResult = 0;

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_INSERT_SAMPLE_TEST";

                        var userIdParam = command.CreateParameter();
                        userIdParam.ParameterName = "@USERS_ID";
                        userIdParam.Value = userId;
                        command.Parameters.Add(userIdParam);

                        var examIdParam = command.CreateParameter();
                        examIdParam.ParameterName = "@EXAM_ID";
                        examIdParam.Value = examId.HasValue && examId.Value != 0 ? examId.Value : DBNull.Value;
                        command.Parameters.Add(examIdParam);

                        var examSubjectIdParam = command.CreateParameter();
                        examSubjectIdParam.ParameterName = "@EXAM_SUBJECT_ID";
                        examSubjectIdParam.Value = examSubjectId.HasValue && examSubjectId.Value != 0 ? examSubjectId.Value : DBNull.Value;
                        command.Parameters.Add(examSubjectIdParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                examIdFromResult = reader.IsDBNull(reader.GetOrdinal("EXAM_ID")) ? null : reader.GetInt32(reader.GetOrdinal("EXAM_ID"));
                                examName = reader.IsDBNull(reader.GetOrdinal("EXAM_NAME")) ? null : reader.GetString(reader.GetOrdinal("EXAM_NAME"));
                                examSubjectIdFromResult = reader.IsDBNull(reader.GetOrdinal("EXAM_SUBJECT_ID")) ? null : reader.GetInt32(reader.GetOrdinal("EXAM_SUBJECT_ID"));
                                duration = reader.GetInt32(reader.GetOrdinal("DURATION"));
                                classIdFromResult = reader.GetInt32(reader.GetOrdinal("CLASS_ID"));
                            }
                            else
                            {
                                return NotFound("No test parameters found for the given user.");
                            }
                        }
                    }

                    int? numberOfQuestions = null;
                    if (examIdFromResult.HasValue)
                    {
                        using (var countCommand = connection.CreateCommand())
                        {
                            countCommand.CommandText = "SELECT TOP 1 NO_OF_QUESTIONS FROM EXAM_SUBJECT WHERE EXAM_ID = @examId";
                            var examIdParam = countCommand.CreateParameter();
                            examIdParam.ParameterName = "@examId";
                            examIdParam.Value = examIdFromResult.Value;
                            countCommand.Parameters.Add(examIdParam);

                            var result = countCommand.ExecuteScalar();
                            numberOfQuestions = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 8;
                        }
                    }
                    else
                    {
                        numberOfQuestions = 8;
                    }

                    var userDetails = new
                    {
                        userId = userId,
                        classId = classIdFromResult,
                        examId = examIdFromResult,
                        examName = examName,
                        examSubjectId = examSubjectIdFromResult,
                        duration = duration,
                        numberOfQuestions = numberOfQuestions
                    };

                    _logger.LogInformation("Returning test params for user {UserId}: {@UserDetails}", userId, userDetails);
                    return Ok(userDetails);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sample test parameters.");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("GetExamResult")]
        public async Task<IActionResult> GetExamResult(int examId, int usersId, int attemptNo)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_GET_EXAM_RESULT";

                        // Input parameters
                        command.Parameters.Add(new SqlParameter("@EXAM_ID", SqlDbType.Int) { Value = examId });
                        command.Parameters.Add(new SqlParameter("@USERS_ID", SqlDbType.Int) { Value = usersId });
                        command.Parameters.Add(new SqlParameter("@ATTEMPT_NO", SqlDbType.Int) { Value = attemptNo });

                        // Execute the stored procedure and read the result set
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var result = new
                                {
                                    TotalQuestions = reader.IsDBNull(reader.GetOrdinal("TOTAL_QUESTIONS")) ? 0 : reader.GetInt32(reader.GetOrdinal("TOTAL_QUESTIONS")),
                                    QuestionsAttempted = reader.IsDBNull(reader.GetOrdinal("QUESTIONS_ATTEMPTED")) ? 0 : reader.GetInt32(reader.GetOrdinal("QUESTIONS_ATTEMPTED")),
                                    CorrectAnswers = reader.IsDBNull(reader.GetOrdinal("CORRECT_ANSWERS")) ? 0 : reader.GetInt32(reader.GetOrdinal("CORRECT_ANSWERS")),
                                    IncorrectAnswers = reader.IsDBNull(reader.GetOrdinal("INCORRECT")) ? 0 : reader.GetInt32(reader.GetOrdinal("INCORRECT")),
                                    MarksScored = reader.IsDBNull(reader.GetOrdinal("MARKS_SCORED")) ? 0 : reader.GetInt32(reader.GetOrdinal("MARKS_SCORED")),
                                    TotalPercentage = reader.IsDBNull(reader.GetOrdinal("TOTAL_PERCENTAGE")) ? 0m : reader.GetDecimal(reader.GetOrdinal("TOTAL_PERCENTAGE"))
                                };

                                return Ok(result);
                            }
                            else
                            {
                                return NotFound("No exam result found for the specified exam, user, and attempt number.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching exam result for examId {ExamId}, usersId {UsersId}, attemptNo {AttemptNo}", examId, usersId, attemptNo);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}