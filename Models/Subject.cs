using System;
using System.ComponentModel.DataAnnotations;

public class Subject
{
    [Key]
    public int SubjectId { get; set; } // Matches SUBJECT_ID in DB

    [Required]
    public string SubjectName { get; set; } = string.Empty; // Matches SUBJECT_NAME

    public int CreatedBy { get; set; } // Matches CREATED_BY

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Matches CREATED_DATE

    public int ModifiedBy { get; set; } // Matches MODIFIED_BY

    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow; // Matches MODIFIED_DATE

    public bool IsActive { get; set; } = true; // Matches IS_ACTIVE

    public int CourseTypeId { get; set; } // Matches COURSE_TYPE_ID
}
