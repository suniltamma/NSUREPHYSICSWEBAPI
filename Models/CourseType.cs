using System.ComponentModel.DataAnnotations.Schema;

[Table("COURSE_TYPE")]  // 🔹 Match the exact table name in the DB
public class CourseType
{
    [Column("COURSE_TYPE_ID")] // 🔹 Match column name in DB
    public int CourseTypeId { get; set; }

    [Column("COURSE_TYPE_NAME")] // 🔹 Match column name in DB
    public string CourseTypeName { get; set; } = string.Empty;
}
