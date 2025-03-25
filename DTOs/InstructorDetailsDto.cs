using System.ComponentModel.DataAnnotations.Schema;

namespace NSurePhysicsWebAPI.DTOs
{
    public class InstructorDetailsDto
    {
        [Column("INSTRUCTOR_ID")]
        public int InstructorId { get; set; }

        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("FIRST_NAME")]
        public string FirstName { get; set; } = string.Empty;

        [Column("LAST_NAME")]
        public string LastName { get; set; } = string.Empty;

        [Column("INSTRUCTOR_SUBJECT_ID")]
        public int InstructorSubjectId { get; set; }

        [Column("INSTRUCTOR_CLASS")]
        public int InstructorClass { get; set; }

        [Column("INSTRUCTOR_COURSE_YIPE_ID")]
        public int InstructorCourseYipeId { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; } = string.Empty;

        [Column("ACHIVEMENTS")]
        public string? Achievements { get; set; }

        [Column("JOINED_DATE")]
        public DateTime JoinedDate { get; set; }

        [Column("TOTAL_EXPERIENCE")]
        public int TotalExperience { get; set; }

        [Column("SOCIAL_MEDIA_LINK")]
        public string? SocialMediaLink { get; set; }

        [Column("QUALIFICATION")]
        public string? Qualification { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
    }
}