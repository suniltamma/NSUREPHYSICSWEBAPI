using System.ComponentModel.DataAnnotations;

public class SaveSubjectDto
{
    public int SubjectId { get; set; } = 0;
    public string SubjectName { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    [Required(ErrorMessage = "UserId is required.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Valid CourseTypeId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "CourseTypeId must be a valid positive number.")]
    public int CourseTypeId { get; set; }
}
