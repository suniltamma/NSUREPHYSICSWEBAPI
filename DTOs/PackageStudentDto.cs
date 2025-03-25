// NSurePhysicsWebAPI/DTOs/PackageStudentDto.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace NSurePhysicsWebAPI.DTOs
{
    public class PackageStudentDto
    {
        [Column("PACKAGE_ID")]
        public int PackageId { get; set; }

        [Column("PACKAGE_NAME")]
        public string PackageName { get; set; } = string.Empty;

        [Column("SUBJECT_ID")]
        public int SubjectId { get; set; }

        [Column("SUBJECT_NAME")]
        public string SubjectName { get; set; } = string.Empty;

        [Column("PACKAGE_SYLLABUS_ID")]
        public int PackageSyllabusId { get; set; }

        [Column("COURSE_TYPE_ID")]
        public int CourseTypeId { get; set; }

        [Column("PACKAGE_NO_DAYS")]
        public int PackageNoDays { get; set; }

        [Column("PACKAGE_COST")]
        public int PackageCost { get; set; } // Changed from decimal to int
    }
}