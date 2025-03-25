using System.ComponentModel.DataAnnotations;

namespace NSurePhysicsWebAPI.Models
{
    public class ExamResult
    {
        [Key]
        public int ExamResultId { get; set; }
        public int ExamQuestionId { get; set; }
        public int UsersId { get; set; }
        public int MarkedAnswers { get; set; }
        public int ExamSubjectId { get; set; }
    }
}
