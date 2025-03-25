using System.ComponentModel.DataAnnotations;

namespace NSurePhysicsWebAPI.Models
{
    public class Topics
    {
        [Key]
        public int TopicsId { get; set; } 
        public int ChapterId { get; set; } // Matches CHAPTER_ID in DB
        public int ClassId { get; set; } 
        public int SubjectId { get; set; } 

        [Required]
        public string TopicsName { get; set; } = string.Empty; 

        public string TopicsDescription { get; set; } = string.Empty;
        public string ChapterName { get; set; } = string.Empty;

        public int CourseTypeId { get; set; }

        public int CreatedBy { get; set; } // Matches CREATED_BY

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Matches CREATED_DATE

        public bool IsActive { get; set; } = true; // Matches IS_ACTIVE

        public int TotalCount { get; set; } 
    }
}
