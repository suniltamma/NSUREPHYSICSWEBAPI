using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class QuestionsDto
{
    [Column("QUESTION_ID")]
    public int QuestionId { get; set; }
    [Column("TOPICS_ID")]
    public int TopicsId { get; set; }
    [Column("QUESTION_TYPICAL_LEVEL_ID")]
    public int Question_Typical_level_Id { get; set; }
    [Column("QUESTION_NAME")]
    public string QuestionName { get; set; } = string.Empty;
    [Column("OPTION_1")]
    public string? Option_1 { get; set; } = string.Empty;
    [Column("OPTION_2")]
    public string? Option_2 { get; set; } = string.Empty;
    [Column("OPTION_3")]
    public string? Option_3 { get; set; } = string.Empty;
    [Column("OPTION_4")]
    public string? Option_4 { get; set; } = string.Empty;
    [Column("QUESTION_ANSWER")]
    public int QuestionAnswer { get; set; } // Already correct
    [Column("SOLUTION_ANALYSIS")]
    public string SolutionAnalysis { get; set; } = string.Empty; // Renamed from Explanaation
    [Column("REPEATED_QUESTION_YEAR")]
    public string Repeated_Question_Year { get; set; } = string.Empty;
    [Column("IS_ACTIVE")]
    public bool IsActive { get; set; }
}

public class SaveQuestionDto
{
    public int QuestionId { get; set; }
    public int Question_Typical_level_Id { get; set; }
    public int TopicsId { get; set; }
    [Required]
    public string QuestionName { get; set; } = string.Empty;
    public string Option_1 { get; set; } = string.Empty;
    public string Option_2 { get; set; } = string.Empty;
    public string Option_3 { get; set; } = string.Empty;
    public string Option_4 { get; set; } = string.Empty;
    public int QuestionAnswer { get; set; } // Already correct
    public string SolutionAnalysis { get; set; } = string.Empty; // Renamed from Explanaation
    public string Repeated_Question_Year { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int UserId { get; set; }
}

