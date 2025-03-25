using System.ComponentModel.DataAnnotations.Schema;

public class SubjectDto
{
    [Column("SUBJECT_ID")] // Matches DB column
    public int SubjectId { get; set; }

    [Column("SUBJECT_NAME")] // Matches DB column
    public string? SubjectName { get; set; } = string.Empty;

    [Column("IS_ACTIVE")] // Matches DB column
    public bool IsActive { get; set; } // Ensure correct data type

    [Column("COURSE_TYPE_NAME")] // Matches DB column
    public string CourseTypeName { get; set; } = string.Empty;

    [Column("TotalCount")] // Matches DB column
    public int TotalCount { get; set; }
}
