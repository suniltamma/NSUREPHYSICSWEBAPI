using System.ComponentModel.DataAnnotations.Schema;

    public class ChapterDto
    {
        [Column("CHAPTER_ID")] // Matches DB column
        public int ChapterId { get; set; }

         [Column("SUBJECT_ID")] // Matches DB column
        public int SubjectId { get; set; }


        [Column("CLASS_ID")] // Matches DB column
        public int ClassId { get; set; }

         [Column("SYLLABUS_ID")] // Matches DB column
        public int SyllabusId { get; set; }

       

        [Column("CHAPTER_NAME")] // Matches DB column
        public string? ChapterName { get; set; } = string.Empty;

        [Column("CHAPTER_DESCRIPTION")] // Matches DB column
        public string ChapterDescription { get; set; } = string.Empty;

        [Column("IS_ACTIVE")] // Matches DB column
        public bool IsActive { get; set; } // Ensure correct data type

        [Column("COURSE_TYPE_ID")] // Matches DB column
        public int CourseTypeId { get; set; }

        //[Column("TotalCount")] // Matches DB column
        //public int TotalCount { get; set; }
}
public class ChapterbyidDto
{
    [Column("CHAPTER_ID")] // Matches DB column
    public int ChapterId { get; set; }

    [Column("SUBJECT_ID")] // Matches DB column
    public int SubjectId { get; set; }


    [Column("CLASS_ID")] // Matches DB column
    public int ClassId { get; set; }

    [Column("SYLLABUS_ID")] // Matches DB column
    public int SyllabusId { get; set; }

    [Column("SYLLABUS_NAME")] // Matches DB column
    public string SyllabusName { get; set; } = string.Empty;

    [Column("CHAPTER_NAME")] // Matches DB column
    public string ChapterName { get; set; } = string.Empty;

    [Column("CHAPTER_DESCRIPTION")] // Matches DB column
    public string ChapterDescription { get; set; } = string.Empty;

    [Column("IS_ACTIVE")] // Matches DB column
    public bool IsActive { get; set; } // Ensure correct data type

    [Column("COURSE_TYPE_ID")] // Matches DB column
    public int CourseTypeId { get; set; }

    //[Column("TotalCount")] // Matches DB column
    //public int TotalCount { get; set; }
}


