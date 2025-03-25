using System.ComponentModel.DataAnnotations.Schema;

public class ClassDto
{
    [Column("ClassId")] // Matches the alias in SP_GET_ALL_CLASS_DETAILS
    public int ClassId { get; set; }

    [Column("ClassName")] // Matches the alias in SP_GET_ALL_CLASS_DETAILS
    public string ClassName { get; set; } = string.Empty;
}