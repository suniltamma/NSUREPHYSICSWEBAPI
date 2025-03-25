using System.ComponentModel.DataAnnotations;

namespace NSurePhysicsWebAPI.Models
{
    public class Questions
    {
        [Key]
        public int QuestionId { get; set; } // Matches QUESTION_ID
        public int Question_Typical_level_Id { get; set; }
        public int TopicsId { get; set; }
        [Required]
        public string QuestionName { get; set; } = string.Empty;
        public string Option_1 { get; set; } = string.Empty;
        public string Option_2 { get; set; } = string.Empty;
        public string Option_3 { get; set; } = string.Empty;
        public string Option_4 { get; set; } = string.Empty;
        public int QuestionAnswer { get; set; } // Changed to int to match SP
        public string SolutionAnalysis { get; set; } = string.Empty; // Renamed to match SOLUTION_ANALYSIS
        public string Repeated_Question_Year { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true; // Matches IS_ACTIVE
    }
}