using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

    public class ExamDto
    {
    [Column("EXAM_ID")] // Matches DB column
    public int ExamId { get; set; }

    [Column("CLASS_ID")] // Matches DB column
    public int ClassId { get; set; }

    [Column("CLASS_NAME")] // Matches DB column
    public string ClassName { get; set; } = string.Empty;

    [Column("DURATION")] // Matches DB column
    public int Duration { get; set; }

    [Column("EXAM_NAME")] // Matches DB column
    public string ExamName { get; set; } = string.Empty;

    [Column("SYLLABUS_NAME")] // Matches DB column
    public string SyllabusName { get; set; } = string.Empty;

    [Column("EXAM_TYPE")] // Matches DB column
    public string ExamType { get; set; } = string.Empty;

    [Column("EXAM_DATE")] // Matches DB column
    public DateTime ExamDate { get; set; } 

    [Column("SYLLABUS_ID")] // Matches DB column
    public int Syllabus_Id { get; set; }

    [Column("EXAM_SUBJECTS_ID")] // Matches DB column
    public string Exam_Subject_Id { get; set; } = string.Empty;

    [Column("COURSE_TYPE_ID")] // Matches DB column
    public int CourseTypeId { get; set; }

    [Column("IS_ACTIVE")] // Matches DB column
    public bool IsActive { get; set; }

}


public class ExamSubjectDto
{
    [Column("EXAM_SUBJECT_ID")] // Matches DB column
    public int examSubjectId { get; set; }

    [Column("SUBJECT_ID")] // Matches DB column
    public int SubjectId { get; set; }

    [Column("TOPICS_ID")] // Matches DB column
    public int TopicsId { get; set; }

    [Column("CHAPTER_ID")] // Matches DB column
    public int chapterId { get; set; }

    [Column("EXAM_ID")] // Matches DB column
    public int ExamId { get; set; }

    [Column("NO_OF_QUESTIONS")] // Matches DB column
    public int no_of_questions { get; set; } 

    [Column("IS_ACTIVE")] // Matches DB column
    public bool IsActive { get; set; } // Ensure correct data type

  
}



public class SaveExamDto
{
    public int examId { get; set; }
    public int Syllabus_Id { get; set; }
    public int ClassId { get; set; }

    [Required]
    public string examName { get; set; } = string.Empty;
    public string examType { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public DateTime createdDate { get; set; }
    public DateTime modifiedDate { get; set; }

    
    public List<string>? exam_Subject { get; set; } = new List<string>();
    public int Duration { get; set; }
    public int CourseTypeId { get; set; }
    public int Exam_Subject_Id { get; set; }
    public bool IsActive { get; set; } = true;
    public int UserId { get; set; }
}

public class ExamSubjectdetailsDto
{
    public int CHAPTER_ID { get; set; }
    public int SUBJECT_ID { get; set; }
    public int TOPIC_ID { get; set; }
    public int NO_OF_QUESTIONS { get; set; }
    public int CREATED_BY { get; set; }
    public int MODIFIED_BY { get; set; }
    public bool IS_ACTIVE { get; set; } = true;
}

