using System.ComponentModel.DataAnnotations.Schema;

namespace NSurePhysicsWebAPI.DTOs
{
    public class ScheduleDto
    {
        [Column("SCHEDULE_ID")]
        public int ScheduleId { get; set; }

        [Column("SCHEDULE_NAME")]
        public string ScheduleName { get; set; } = null!;

        [Column("PACKAGE_ID")]
        public int PackageId { get; set; }

        [Column("SCHEDULE_DAY_NUMBER")]
        public int ScheduleDayNumber { get; set; }

        [Column("SCHEDULE_DATE")]
        public DateTime ScheduleDate { get; set; }

        [Column("START_TIME")]
        public TimeSpan StartTime { get; set; }

        [Column("END_TIME")]
        public TimeSpan EndTime { get; set; }

        [Column("DURATION")]
        public int Duration { get; set; }

        [Column("TOPICS_ID")]
        public int TopicsId { get; set; }

        [Column("INSTRUCTOR_ID")]
        public int InstructorId { get; set; }

        [Column("LIVE_CLASS_URL")]
        public string? LiveClassUrl { get; set; }

        [Column("CREATED_BY")]
        public int CreatedBy { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }

        [Column("MODIFIED_BY")]
        public int ModifiedBy { get; set; }

        [Column("MODIFIED_DATE")]
        public DateTime ModifiedDate { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }

        [Column("PACKAGE_SUBJECT_ID")]
        public int PackageSubjectId { get; set; }

        [Column("SUBJECT_NAME")]
        public string SubjectName { get; set; } = null!;

        [Column("INSTRUCTOR_NAME")]
        public string InstructorName { get; set; } = null!;
    }

    public class ScheduleDetailsDto
    {
        [Column("SCHEDULE_NAME")]
        public string ScheduleName { get; set; } = null!;

        [Column("SCHEDULE_DAY_NUMBER")]
        public int ScheduleDayNumber { get; set; }

        [Column("SCHEDULE_DATE")]
        public DateTime ScheduleDate { get; set; }

        [Column("START_TIME")]
        public TimeSpan StartTime { get; set; }

        [Column("END_TIME")]
        public TimeSpan EndTime { get; set; }

        [Column("DURATION")]
        public int Duration { get; set; }

        [Column("SUBJECT_NAME")]
        public string SubjectName { get; set; } = null!;

        [Column("CHAPTER_NAME")]
        public string ChapterName { get; set; } = null!;

        [Column("TOPICS_NAME")]
        public string TopicsName { get; set; } = null!;

        [Column("LIVE_CLASS_URL")]
        public string? LiveClassUrl { get; set; }

        [Column("INSTRUCTOR_NAME")]
        public string InstructorName { get; set; } = null!;
    }

    public class TopicAttachmentDto
    {
        [Column("TOPIC_ATTACHMENTS_ID")]
        public int TopicAttachmentsId { get; set; }

        [Column("TOPIC_ATTACHMENT_FILE_NAME")]
        public string TopicAttachmentFileName { get; set; } = null!;
    }

    public class ScheduleDetailsbyIdDto
    {
        [Column("SCHEDULE_ID")]
        public int ScheduleId { get; set; }

        [Column("SCHEDULE_NAME")]
        public string ScheduleName { get; set; } = null!;

        [Column("PACKAGE_ID")]
        public int PackageId { get; set; }

        [Column("SCHEDULE_DAY_NUMBER")]
        public int ScheduleDayNumber { get; set; }

        [Column("SCHEDULE_DATE")]
        public DateTime ScheduleDate { get; set; }

        [Column("START_TIME")]
        public TimeSpan StartTime { get; set; }

        [Column("END_TIME")]
        public TimeSpan EndTime { get; set; }

        [Column("DURATION")]
        public int Duration { get; set; }

        [Column("TOPICS_ID")]
        public int TopicsId { get; set; }

        [Column("INSTRUCTOR_ID")]
        public int InstructorId { get; set; }

        [Column("LIVE_CLASS_URL")]
        public string? LiveClassUrl { get; set; }

        [Column("CREATED_BY")]
        public int CreatedBy { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }

        [Column("MODIFIED_BY")]
        public int ModifiedBy { get; set; }

        [Column("MODIFIED_DATE")]
        public DateTime ModifiedDate { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }

        [Column("PACKAGE_SUBJECT_ID")]
        public int PackageSubjectId { get; set; }

        [Column("SUBJECT_ID")]
        public int SubjectId { get; set; }


    }

    public class SaveScheduleDto
    {
        public int UserId { get; set; }
        public List<string>? Schedules { get; set; } = new List<string>();
    }

    public class SaveScheduleDetailsDto
    {

        public string ScheduleName { get; set; }
        public int PackageId { get; set; }
        public int ScheduleDayNo { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }
        public int TopicsId { get; set; }
        public int InstructorId { get; set; }
        public string LiveClassUrl { get; set; }
        public int PackageSubjectId { get; set; }

    }


    public class UpdateScheduleDetailsDto
    {

        public int Schedule_Id { get; set; }
        public string Schedule_Name { get; set; }
        public DateTime Schedule_Date { get; set; }
        public int Schedule_Day_Number { get; set; }
        public TimeSpan Start_Time { get; set; }
        public TimeSpan End_Time { get; set; }
        public int Duration { get; set; }
        public int Package_Id { get; set; }
        public int Package_Subject_Id { get; set; }
        public int Subject_Id { get; set; }
        public int Topics_Id { get; set; }
        public int Instructor_Id { get; set; }
        public string? Live_Class_Url { get; set; }
        public bool Is_Active { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }

        public int Delete_Flag { get; set; }

    }

}

