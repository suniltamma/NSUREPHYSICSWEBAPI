using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NSurePhysicsWebAPI.Models
{
    public class SampleQuestion // Keeping the existing class name
    {
        [Key]
        public int QUESTION_ID { get; set; } // Updated property names (match DB columns)
        public string REPEATED_QUESTION_YEAR { get; set; }
        public string QUESTION_NAME { get; set; }
        public int QUESTION_ANSWER { get; set; }
        public string SOLUTION_ANALYSIS { get; set; }
        public string OPTION_1 { get; set; }
        public string OPTION_2 { get; set; }
        public string OPTION_3 { get; set; }
        public string OPTION_4 { get; set; }
        public int TOPICS_ID { get; set; }
        public int QUESTION_TYPICAL_LEVEL_ID { get; set; }
        // ... any other fields from the QUESTION table, if necessary
    }
}