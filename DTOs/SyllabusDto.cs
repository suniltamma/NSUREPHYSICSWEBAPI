using System.ComponentModel.DataAnnotations.Schema;

    public class SyllabusDto
    {
        [Column("SYLLABUS_ID")] // Matches DB column
        public int SyllabusId { get; set; }


        [Column("SYLLABUS_NAME")] // Matches DB column
        public string SyllabusName { get; set; } = string.Empty;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
    }

