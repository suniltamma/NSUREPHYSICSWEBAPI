using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSurePhysicsWebAPI.Models;
using NSurePhysicsWebAPI.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using static TopicsDto;
using System.Text;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<CourseType> CourseTypes { get; set; } = null!;
    public DbSet<SampleQuestion> Questions { get; set; }
    public DbSet<PdfFileDto> PdfFiles { get; set; }



    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PdfFileDto>().HasNoKey();
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CourseType>().ToTable("COURSE_TYPE");
        modelBuilder.Entity<SampleQuestion>().ToTable("QUESTION");
    }

    public async Task<List<VideoFilesDto>> GetVideoFilesAsync(bool isSample = false)
    {
        var parameters = new List<SqlParameter>
    {
        new SqlParameter("@IsSample", isSample)
    };

        try
        {
            var result = await this.Database
                .SqlQueryRaw<VideoFilesDto>(
                    "EXEC dbo.SP_GET_VIDEO_FILES @IsSample",
                    parameters.ToArray())
                .ToListAsync();

            return result ?? new List<VideoFilesDto>(); // Return empty list if null
        }
        catch (Exception ex)
        {
            throw new Exception($"Error executing SP_GET_VIDEO_FILES: {ex.Message}", ex);
        }
    }

    // In ApplicationDbContext.cs, add this method
    public async Task<List<InstructorDetailsDto>> GetInstructorDetailsAsync(int? instructorId = null, int? userId = null, bool? isActive = null)
    {
        var parameters = new List<SqlParameter>
    {
        new SqlParameter("@InstructorId", instructorId.HasValue ? instructorId : DBNull.Value),
        new SqlParameter("@UserId", userId.HasValue ? userId : DBNull.Value),
        new SqlParameter("@IsActive", isActive.HasValue ? isActive : DBNull.Value)
    };

        return await this.Database
            .SqlQueryRaw<InstructorDetailsDto>(
                "EXEC dbo.SP_GET_INSTRUCTOR_DETAILS @InstructorId, @UserId, @IsActive",
                parameters.ToArray())
            .ToListAsync();
    }

    public async Task<List<SiteContentDto>> GetAllSiteContentsAsync()
    {
        return await this.Database
            .SqlQueryRaw<SiteContentDto>("EXEC dbo.SP_GET_ALL_SITE_CONTENTS")
            .ToListAsync();
    }

    public async Task<int> SaveSiteContentAsync(SiteContentDto content)
    {
        return await this.Database.ExecuteSqlRawAsync(
            "EXEC dbo.SP_SAVE_SITE_CONTENT @CONTENTID, @CONTENTIDNAME, @CONTENTIDDESC, @CONTENTIDIMAGEURL, @IMAGE_HEIGHT, @IMAGE_WIDTH, @ISACTIVE",
            new SqlParameter("@CONTENTID", content.ContentId),
            new SqlParameter("@CONTENTIDNAME", content.ContentName),
            new SqlParameter("@CONTENTIDDESC", content.ContentDesc),
            new SqlParameter("@CONTENTIDIMAGEURL", content.ContentImageUrl ?? (object)DBNull.Value),
            new SqlParameter("@IMAGE_HEIGHT", content.ImageHeight),
            new SqlParameter("@IMAGE_WIDTH", content.ImageWidth),
            new SqlParameter("@ISACTIVE", content.IsActive)
        );
    }


    // PhotoGallery
    public async Task<List<PhotoGalleryDto>> GetAllPhotosAsync()
    {
        return await this.Database
            .SqlQueryRaw<PhotoGalleryDto>("EXEC dbo.SP_GET_ALL_PHOTOS")
            .ToListAsync();
    }

    //  Fetch all classes using stored procedure
    public async Task<List<ClassDto>> GetAllClassesAsync()
    {
        return await this.Database.SqlQueryRaw<ClassDto>("EXEC dbo.SP_GET_ALL_CLASS").ToListAsync();
    }

    //  Fetch all classes details using stored procedure
    public async Task<List<ClassDto>> GetAllClassesdetailsAsync()
    {
        return await this.Database.SqlQueryRaw<ClassDto>("EXEC SP_GET_ALL_CLASS_DETAILS").ToListAsync();
    }

    public async Task<List<SyllabusDto>> GetAllSyllabusAsync()
    {
        return await this.Database.SqlQueryRaw<SyllabusDto>("EXEC dbo.SP_GET_ALL_SYLLABUS").ToListAsync();
    }
    // ✅ Fetch all classes using stored procedure
    //public async Task<List<string>> GetAllClassesAsync()
    //{
    //    return await this.Database.SqlQueryRaw<string>("EXEC dbo.SP_GET_ALL_CLASS").ToListAsync();
    //}

    // ✅ Fetch all subjects using stored procedure
    public async Task<List<SubjectDto>> GetAllSubjectsAsync()
    {
        return await this.Database.SqlQueryRaw<SubjectDto>("EXEC dbo.SP_GET_ALL_SUBJECTS").ToListAsync();
    }

    // ✅ Save subject using stored procedure
    public async Task<int> SaveSubjectAsync(SaveSubjectDto subject)
    {
        return await this.Database.ExecuteSqlRawAsync(
            "EXEC dbo.SP_SAVE_SUBJECTS @p0, @p1, @p2, @p3, @p4",
            subject.SubjectId, subject.SubjectName, subject.IsActive, subject.UserId, subject.CourseTypeId
        );
    }

    public async Task<List<SubjectDto>> GetAllSubjectsAsync(int pageNumber, int pageSize)
    {
        // Adding parameters to the stored procedure call
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);

        return await this.Database.SqlQueryRaw<SubjectDto>(
            "EXEC dbo.SP_GET_ALL_SUBJECTS @PageNumber, @PageSize",
            pageNumberParam, pageSizeParam).ToListAsync();
    }


    public async Task<List<ChapterbyidDto>> GetAllChaptersIdAsync(int classId , int subjectID, int pageNumber, int pageSize)
    {
        // Adding parameters to the stored procedure call
        var classIdParam = new SqlParameter("@CLASS_ID", classId);
        var subjectIDParm = new SqlParameter("@SUBJECT_ID", subjectID);
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);
       


        return await this.Database.SqlQueryRaw<ChapterbyidDto>(
            "EXEC dbo.GRIED_GET_CHAPTER @CLASS_ID,@SUBJECT_ID,@PageNumber, @PageSize",
            classIdParam, subjectIDParm,pageNumberParam, pageSizeParam).ToListAsync();
    }

    public async Task<List<TopicsDto>> GetAllTopicsByIdAsync(int? chapterID, int pageNumber, int pageSize)
    {
        // Adding parameters to the stored procedure call
        var chapterIDParm = new SqlParameter("@CHAPTER_ID", chapterID.HasValue ? (object)chapterID.Value : DBNull.Value);
        var pageNumberParam = new SqlParameter("@PAGENUMBER", pageNumber);
        var pageSizeParam = new SqlParameter("@PAGESIZE", pageSize);



        return await this.Database.SqlQueryRaw<TopicsDto>(
            "EXEC dbo.SP_GRIED_GET_TOPICS @CHAPTER_ID,@PAGENUMBER, @PAGESIZE",
             chapterIDParm, pageNumberParam, pageSizeParam).ToListAsync();
    }

    public async Task<List<TopicsBySubID>> GetAllTopicBySubjectID(int SubjectID)
    {
        // Adding parameters to the stored procedure call
        var SubjectIDParam = new SqlParameter("@SubjectID", SubjectID);

        return await this.Database.SqlQueryRaw<TopicsBySubID>(
            "EXEC SP_GET_TOPICS_BY_SUBJECT_ID @SubjectID",
            SubjectIDParam).ToListAsync();
    }


    //  Get Questions using stored procedure
    public async Task<List<QuestionsDto>> GetAllQuestionsAsync()
    {
        return await this.Database.SqlQueryRaw<QuestionsDto>("EXEC dbo.SP_GET_ALL_QUESTION").ToListAsync();
    }

    //  Save chapter using stored procedure
    public async Task<int> SavechapterAsync(SaveChapterDto chapter)
    {
        return await this.Database.ExecuteSqlRawAsync(
            "EXEC SP_SAVE_CHAPTER @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8", 
            chapter.ChapterId, chapter.ChapterName, chapter.ChapterDescription, chapter.ClassId, chapter.SubjectId,
            chapter.SyllabusId, chapter.CourseTypeId, chapter.UserId, chapter.IsActive
        );
    }

    //  Save topic using stored procedure
    //public async Task<int> SaveTopicAsync(SaveTopicDto topic)
    //{
    //    return await this.Database.ExecuteSqlRawAsync(
    //        "EXEC SP_SAVE_TOPICS @p0, @p1, @p2, @p3, @p4, @p5",
    //        topic.TopicsId, topic.TopicsName, topic.TopicsDescription, topic.ChapterId, topic.UserId,topic.IsActive
    //    );
    //}
    public async Task<int> SaveTopicAsync(SaveTopicDto topic)
    {
        // Generate XML for the attachments
        string topicAttachmentsXml = GenerateTopicAttachmentsXml(topic.FilePaths);

        // Execute the stored procedure with the XML parameter
        return await this.Database.ExecuteSqlRawAsync(
            "EXEC SP_SAVE_TOPICS @p0, @p1, @p2, @p3, @p4, @p5, @p6",
            topic.TopicsId, topic.TopicsName, topic.TopicsDescription, topic.ChapterId, topic.UserId, topic.IsActive, topicAttachmentsXml
        );
    }

    // Generate XML for the file attachments
    private string GenerateTopicAttachmentsXml(List<string> filePaths)
    {
        if (filePaths == null || filePaths.Count == 0)
            return "<TOPIC_ATTACHMENTS></TOPIC_ATTACHMENTS>";  // Empty XML if no files

        StringBuilder xmlBuilder = new StringBuilder();
        xmlBuilder.Append("<TOPIC_ATTACHMENTS>");

        foreach (var filePath in filePaths)
        {
            string fileName = Path.GetFileName(filePath);
            xmlBuilder.Append($"<RESULT TOPIC_ATTACHMENT_FILE_NAME=\"{fileName}\" />");
        }

        xmlBuilder.Append("</TOPIC_ATTACHMENTS>");
        return xmlBuilder.ToString();
    }

    //  Save topic using stored procedure
    public async Task<int> SaveQuestionsAsync(SaveQuestionDto question)
    {
        return await this.Database.ExecuteSqlRawAsync(
            "EXEC SP_SAVE_QUESTION @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12",
            question.QuestionId, question.QuestionName, question.Option_1, question.Option_2, question.Option_3,
            question.Option_4, question.QuestionAnswer, question.SolutionAnalysis, question.Repeated_Question_Year,
            question.TopicsId, question.Question_Typical_level_Id, question.UserId, question.IsActive
        );
    }

    private static string GenerateExamSubjectsXml(List<ExamSubjectDto> subjects)
    {
        if (subjects == null || subjects.Count == 0)
            return "<EXAM_SUBJECT></EXAM_SUBJECT>";

        StringBuilder xmlBuilder = new StringBuilder();
        xmlBuilder.Append("<EXAM_SUBJECT>");

        //foreach (var subject in subjects)
        //{
        //    xmlBuilder.Append($"<RESULT CHAPTER_ID=\"{subject.chapterId}\" SUBJECT_ID=\"{subject.SubjectId}\" TOPIC_ID=\"{subject.TopicsId}\" NO_OF_QUESTIONS=\"{subject.NoOfQuestions}\" CREATED_BY=\"{subject.CreatedBy}\" MODIFIED_BY=\"{subject.ModifiedBy}\" IS_ACTIVE=\"{subject.IsActive}\" />");
        //}

        xmlBuilder.Append("</EXAM_SUBJECT>");
        return xmlBuilder.ToString();
    }


    // ✅ Register user using stored procedure
    public async Task<int> RegisterUserAsync(UserRegistrationDto userDto)
    {
        var parameters = new[]
        {
            new SqlParameter("@FirstName", userDto.FirstName),
            new SqlParameter("@LastName", userDto.LastName),
            new SqlParameter("@Email", userDto.Email),
            new SqlParameter("@Password", userDto.Password),
            new SqlParameter("@PhoneNumber", userDto.PhoneNumber ?? (object)DBNull.Value),
            new SqlParameter("@Address", userDto.Address ?? (object)DBNull.Value),
            new SqlParameter("@City", userDto.City ?? (object)DBNull.Value),
            new SqlParameter("@State", userDto.State ?? (object)DBNull.Value),
            new SqlParameter("@ClassId", userDto.ClassId),
            new SqlParameter("@IMAGE_PATH", userDto.ImagePath ?? (object)DBNull.Value),
            new SqlParameter("@RoleId", 2) { Value = 2 }, // Default from SP
            new SqlParameter("@IsActive", true) { Value = true }, // Default from SP
            new SqlParameter("@CreatedBy", userDto.CreatedBy),
            new SqlParameter("@CreatedDate", DateTime.UtcNow),
            new SqlParameter("@ModifiedDate", DateTime.UtcNow),
            new SqlParameter("@UserId", SqlDbType.Int) { Direction = ParameterDirection.Output }
        };

        await Database.ExecuteSqlRawAsync(
            "EXEC @UserId = dbo.SP_REGISTER_USER @FirstName, @LastName, @Email, @Password, @PhoneNumber, @Address, @City, @State, @ClassId, @IMAGE_PATH, @RoleId, @IsActive, @CreatedBy, @CreatedDate, @ModifiedDate",
            parameters);

        return (int)parameters[15].Value; // Return the output UserId
    }



    public async Task<List<ClassDto>> GetAllClassesWithDetailsAsync()
    {
        return await this.Database.SqlQueryRaw<ClassDto>("EXEC dbo.SP_GET_ALL_CLASS_DETAILS").ToListAsync();
    }


    public async Task<List<TopicAttachmentDto>> GetTopicAttachmentsAsync(int userId)
    {
        var userIdParam = new SqlParameter("@USERS_ID", userId);
        return await this.Database
            .SqlQueryRaw<TopicAttachmentDto>("EXEC SP_TOPIC_ATTACHMENTS @USERS_ID", userIdParam)
            .ToListAsync();
    }
   
    public async Task<List<TopicsAttactmentsDTO>> GetAllTopicsAttachmentsById(int TopicsID)
    {
        // Adding parameters to the stored procedure call
        var TopicsIdParam = new SqlParameter("@TOPICS_ID", TopicsID);
        return await this.Database.SqlQueryRaw<TopicsAttactmentsDTO>(
            "EXEC dbo.GET_TOPIC_ATTACHMENTS_BY_TOPICID @TOPICS_ID", TopicsIdParam).ToListAsync();
    }

    public async Task<(List<ExamDto>, List<ExamSubjectDto>)> GetAllExamsByIdAsync(int UserID)
    {
        var exams = new List<ExamDto>();
        var examSubjects = new List<ExamSubjectDto>();

        // Create a connection and command
        await using (var connection = new SqlConnection(this.Database.GetDbConnection().ConnectionString))
        {
            await connection.OpenAsync();

            // Create the command for the stored procedure
            await using (var command = new SqlCommand("dbo.SP_GET_EXAM", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@USERS_ID", UserID));

                // Execute the command and get the reader
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    // Read the first result set (Exam details)
                    while (await reader.ReadAsync())
                    {
                        var exam = new ExamDto
                        {
                            ExamId = reader.GetInt32(reader.GetOrdinal("EXAM_ID")),
                            ExamName = reader.GetString(reader.GetOrdinal("EXAM_NAME")),
                            Syllabus_Id = reader.GetInt32(reader.GetOrdinal("SYLLABUS_ID")),
                            SyllabusName = reader.GetString(reader.GetOrdinal("SYLLABUS_NAME")),
                            ClassId = reader.GetInt32(reader.GetOrdinal("CLASS_ID")),
                            ClassName = reader.GetString(reader.GetOrdinal("CLASS_NAME")),
                            ExamType = reader.GetString(reader.GetOrdinal("EXAM_TYPE")),
                            ExamDate = reader.GetDateTime(reader.GetOrdinal("EXAM_DATE")),
                            Duration = reader.GetInt32(reader.GetOrdinal("DURATION")),
                            CourseTypeId = reader.GetInt32(reader.GetOrdinal("COURSE_TYPE_ID")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IS_ACTIVE")),
                            Exam_Subject_Id = reader.GetString(reader.GetOrdinal("EXAM_SUBJECTS_ID"))
                        };
                        exams.Add(exam);
                    }

                    // Move to the second result set (Exam Subjects)
                    await reader.NextResultAsync();

                    // Read the second result set
                    while (await reader.ReadAsync())
                    {
                        var examSubject = new ExamSubjectDto
                        {
                            examSubjectId = reader.GetInt32(reader.GetOrdinal("EXAM_SUBJECT_ID")),
                            ExamId = reader.GetInt32(reader.GetOrdinal("EXAM_ID")),
                            // SubjectName = reader.GetString(reader.GetOrdinal("SUBJECT_NAME")),
                            TopicsId = reader.GetInt32(reader.GetOrdinal("TOPICS_ID")),
                            chapterId = reader.GetInt32(reader.GetOrdinal("CHAPTER_ID")),
                            no_of_questions = reader.GetInt32(reader.GetOrdinal("NO_OF_QUESTIONS")),
                            SubjectId = reader.GetInt32(reader.GetOrdinal("SUBJECT_ID")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IS_ACTIVE"))

                            // Add other properties as needed
                        };
                        examSubjects.Add(examSubject);
                    }
                }
            }
        }

        return (exams, examSubjects);
    }

    public async Task<(List<PackageDto>, List<PackageSubjectDto>)> GetAllPackagesByIdAsync(int UserID)
    {
        var packages = new List<PackageDto>();
        var packageSubjects = new List<PackageSubjectDto>();

        // Create a connection and command
        await using (var connection = new SqlConnection(this.Database.GetDbConnection().ConnectionString))
        {
            await connection.OpenAsync();

            // Create the command for the stored procedure
            await using (var command = new SqlCommand("dbo.SP_GET_PACKAGE", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@USERS_ID", UserID));

                // Execute the command and get the reader
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    // Read the first result set (Exam details)
                    while (await reader.ReadAsync())
                    {
                        var package = new PackageDto
                        {

                            PackageId = reader.GetInt32(reader.GetOrdinal("PACKAGE_ID")),
                            PackageName = reader.GetString(reader.GetOrdinal("PACKAGE_NAME")),
                            Package_Syllabus_Id = reader.GetInt32(reader.GetOrdinal("PACKAGE_SYLLABUS_ID")),

                            Package_No_Days = reader.GetInt32(reader.GetOrdinal("PACKAGE_NO_DAYS")),

                            PackageCost = reader.GetInt32(reader.GetOrdinal("PACKAGE_COST")),
                            CourseTypeId = reader.GetInt32(reader.GetOrdinal("COURSE_TYPE_ID")),
                            //IsActive = reader.GetBoolean(reader.GetOrdinal("IS_ACTIVE")),
                            //Exam_Subject_Id = reader.GetString(reader.GetOrdinal("EXAM_SUBJECTS_ID"))
                        };
                        packages.Add(package);
                    }

                    // Move to the second result set (Exam Subjects)
                    await reader.NextResultAsync();

                    // Read the second result set
                    while (await reader.ReadAsync())
                    {
                        var packageSubject = new PackageSubjectDto
                        {
                            package_subject_id = reader.GetInt32(reader.GetOrdinal("PACKAGE_SUBJECTS_ID")),
                            packageId = reader.GetInt32(reader.GetOrdinal("PACKAGE_ID")),
                            // SubjectName = reader.GetString(reader.GetOrdinal("SUBJECT_NAME")),
                            SubjectId = reader.GetInt32(reader.GetOrdinal("SUBJECT_ID")),
                            subject_Name = reader.GetString(reader.GetOrdinal("SUBJECT_NAME")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IS_ACTIVE")),


                            // Add other properties as needed
                        };
                        packageSubjects.Add(packageSubject);
                    }
                }
            }
        }
        return (packages, packageSubjects);
    }

    public async Task<List<PdfFileDto>> GetPdfFilesAsync(int chapterId, int subjectId, int pageNumber, int pageSize)
    {
        return await this.PdfFiles
            .FromSqlRaw("EXEC SP_GET_TOPIC_ATTACHMENTS @CHAPTER_ID={0}, @SUBJECT_ID={1}, @PAGENUMBER={2}, @PAGESIZE={3}",
                chapterId, subjectId, pageNumber, pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PackageStudentDto>> GetPackagesAsync(int userId)
    {
        var userIdParam = new SqlParameter("@USERS_ID", userId);
        return await this.Database
            .SqlQueryRaw<PackageStudentDto>("EXEC SP_GET_PACKAGE_STUDENT @USERS_ID", userIdParam)
            .ToListAsync();
    }

    public async Task<List<ScheduleDto>> GetUserPackageScheduleAsync(int userId)
    {
        var userIdParam = new SqlParameter("@USER_ID", userId);
        return await this.Database
            .SqlQueryRaw<ScheduleDto>("EXEC SP_GET_USER_PACKAGE_SCHEDULE @USER_ID", userIdParam)
            .ToListAsync();
    }

    public async Task<(NSurePhysicsWebAPI.DTOs.ScheduleDetailsDto? Schedule, List<NSurePhysicsWebAPI.DTOs.TopicAttachmentDto> Attachments)> GetUserPackageScheduleDetailsAsync(int userId, int scheduleId)
    {
        using var connection = new SqlConnection(Database.GetDbConnection().ConnectionString);
        using var command = new SqlCommand("SP_GET_USER_PACKAGE_SCHEDULE_DETAILS", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@USER_ID", userId);
        command.Parameters.AddWithValue("@SCHEDULE_ID", scheduleId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        // First result set: Schedule details
        NSurePhysicsWebAPI.DTOs.ScheduleDetailsDto? schedule = null;
        if (await reader.ReadAsync())
        {
            schedule = new NSurePhysicsWebAPI.DTOs.ScheduleDetailsDto
            {
                ScheduleName = reader.GetString("SCHEDULE_NAME"),
                ScheduleDayNumber = reader.GetInt32("SCHEDULE_DAY_NUMBER"),
                ScheduleDate = reader.GetDateTime("SCHEDULE_DATE"),
                StartTime = (TimeSpan)reader["START_TIME"], // Direct cast to TimeSpan
                EndTime = (TimeSpan)reader["END_TIME"],     // Direct cast to TimeSpan
                Duration = reader.GetInt32("DURATION"),
                SubjectName = reader.GetString("SUBJECT_NAME"),
                ChapterName = reader.GetString("CHAPTER_NAME"),
                TopicsName = reader.GetString("TOPICS_NAME"),
                LiveClassUrl = reader.IsDBNull("LIVE_CLASS_URL") ? null : reader.GetString("LIVE_CLASS_URL"),
                InstructorName = reader.GetString("INSTRUCTOR_NAME")
            };
        }

        // Second result set: Topic attachments
        var attachments = new List<NSurePhysicsWebAPI.DTOs.TopicAttachmentDto>();
        if (await reader.NextResultAsync())
        {
            while (await reader.ReadAsync())
            {
                attachments.Add(new NSurePhysicsWebAPI.DTOs.TopicAttachmentDto
                {
                    TopicAttachmentsId = reader.GetInt32("TOPIC_ATTACHMENTS_ID"),
                    TopicAttachmentFileName = reader.GetString("TOPIC_ATTACHMENT_FILE_NAME")
                });
            }
        }

        return (schedule, attachments);
    }

    public async Task<List<ChapterDto>> GetAllChaptersAsync()
    {
        return await this.Database.SqlQueryRaw<ChapterDto>("EXEC dbo.SP_GET_ALL_CHAPTER").ToListAsync();
    }

    public async Task<List<ScheduleDetailsbyIdDto>> GetAllScheduleByPackageIDAsync(int packageID)
    {
        var packageIdParam = new SqlParameter("@PACKAGE_ID", packageID);
        return await this.Database.SqlQueryRaw<ScheduleDetailsbyIdDto>("EXEC SP_GET_PACKAGE_SCHEDULE @PACKAGE_ID", packageIdParam).ToListAsync();
    }

    public async Task<int> UpdateScheduleByScheIDAsync(UpdateScheduleDetailsDto schedule)
    {
        return await this.Database.ExecuteSqlRawAsync(
        "EXEC dbo.SP_UPDATE_PACKAGE_SCHEDULE_BY_ID @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15,@p16",
        schedule.Schedule_Id,
        schedule.Schedule_Name,
        schedule.Schedule_Date,
        schedule.Schedule_Day_Number,
        schedule.Start_Time,
        schedule.End_Time,
        schedule.Duration,
        schedule.Package_Id,
        schedule.Package_Subject_Id,
        schedule.Subject_Id,
        schedule.Topics_Id,
        schedule.Instructor_Id,
        schedule.Live_Class_Url,
        schedule.Is_Active,
        schedule.Modified_By,
        schedule.Modified_Date,
        schedule.Delete_Flag
    );
    }

}
