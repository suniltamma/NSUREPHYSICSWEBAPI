using System.ComponentModel.DataAnnotations;

namespace NSurePhysicsWebAPI.Models
{
    public class Chapter
    {
        [Key]
        public int ChapterId { get; set; } // Matches CHAPTER_ID in DB
        public int SubjectId { get; set; } // Matches CHAPTER_ID in DB

        public int ClassId { get; set; } // Matches Class_ID in DB
        public int SyllabusId { get; set; } // Matches SyllabusId in DB

        [Required]
        public string ChapterName { get; set; } = string.Empty; // Matches CHAPTER_NAME

        //public string SyllabusName { get; set; } = string.Empty;

        public string ChapterDescription { get; set; } = string.Empty;

        public int CreatedBy { get; set; } // Matches CREATED_BY

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Matches CREATED_DATE

        public int ModifiedBy { get; set; } // Matches MODIFIED_BY

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow; // Matches MODIFIED_DATE

        public bool IsActive { get; set; } = true; // Matches IS_ACTIVE

        public int CourseTypeId { get; set; } // Matches COURSE_TYPE_ID

       

    }

    public class Chapterbyid
    {
        [Key]
        public int ChapterId { get; set; } // Matches CHAPTER_ID in DB
        public int SubjectId { get; set; } // Matches CHAPTER_ID in DB

        public int ClassId { get; set; } // Matches Class_ID in DB
        public int SyllabusId { get; set; } // Matches SyllabusId in DB

        [Required]
        public string ChapterName { get; set; } = string.Empty; // Matches CHAPTER_NAME

        public string SyllabusName { get; set; } = string.Empty;

        public string ChapterDescription { get; set; } = string.Empty;

        public int CreatedBy { get; set; } // Matches CREATED_BY

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Matches CREATED_DATE

        public int ModifiedBy { get; set; } // Matches MODIFIED_BY

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow; // Matches MODIFIED_DATE

        public bool IsActive { get; set; } = true; // Matches IS_ACTIVE

        public int CourseTypeId { get; set; } // Matches COURSE_TYPE_ID
    }
}
