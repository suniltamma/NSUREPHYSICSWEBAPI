using System.ComponentModel.DataAnnotations;

    public class SaveChapterDto
    {
        public int ChapterId { get; set; } = 0;
        public string ChapterName { get; set; } = string.Empty;

        public string ChapterDescription { get; set; } = string.Empty;

        public int ClassId { get; set; } = 0;
        public int SubjectId { get; set; } = 0;

        public int SyllabusId { get; set; } = 0;

        [Required(ErrorMessage = "Valid CourseTypeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseTypeId must be a valid positive number.")]
        public int CourseTypeId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        public bool IsActive { get; set; }

        

        
        
    }

