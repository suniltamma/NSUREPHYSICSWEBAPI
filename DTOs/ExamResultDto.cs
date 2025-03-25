namespace NSurePhysicsWebAPI.DTOs
{
    public class ExamResultDto
    {
        public int ExamQuestionId { get; set; }
        public int UserId { get; set; }
        public int MarkedAnswer { get; set; }
        public int ExamSubjectId { get; set; }
        public int ExamId { get; set; }
    }
}