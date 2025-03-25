using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

    public class PackageDto
    {

    [Column("PACKAGE_ID")] // Matches DB column
    public int PackageId { get; set; }

    [Column("PACKAGE_COST")] // Matches DB column
    public int PackageCost { get; set; }

    [Column("PACKAGE_NO_DAYS")] // Matches DB column
    public int Package_No_Days { get; set; }

    [Column("PACKAGE_NAME")] // Matches DB column
    public string PackageName { get; set; } = string.Empty;

    //[Column("SUBJECT_NAME")] // Matches DB column
    //public string SubjectName { get; set; } = string.Empty;

    //[Column("PACKAGE_SUBJECT_ID")] // Matches DB column
    //public string Package_Subject_Id { get; set; } = string.Empty;

    [Column("PACKAGE_SYLLABUS_ID")] // Matches DB column
    public int Package_Syllabus_Id { get; set; }

    [Column("COURSE_TYPE_ID")] // Matches DB column
    public int CourseTypeId { get; set; }

    }

public class SavePackageDto
{
    public int PackageId { get; set; }
    public int PackageCost { get; set; }
    public int Package_Syllabus_Id { get; set; }

    [Required]
    public string PackageName { get; set; } = string.Empty;
    public List<string>? package_Subject { get; set; }
    public int Package_No_Days { get; set; }
    public int CourseTypeId { get; set; }
    public bool IsActive { get; set; } = true;
    public int UserId { get; set; }
}


public class PackageSubjectDto
{
    [Column("PACKAGE_SUBJECTS_ID")] // Matches DB column
    public int package_subject_id { get; set; }

    [Column("PACKAGE_ID")] // Matches DB column
    public int packageId { get; set; }

    [Column("SUBJECT_ID")] // Matches DB column
    public int SubjectId { get; set; } 

    [Column("SUBJECT_NAME")] // Matches DB column
    public string subject_Name { get; set; } = string.Empty;


    [Column("IS_ACTIVE")] // Matches DB column
    public bool IsActive { get; set; } // Ensure correct data type


}

public class PackageSubjectdetailsDto
{
   
    public int SUBJECT_ID { get; set; }
    public int CREATED_BY { get; set; }
    public int MODIFIED_BY { get; set; }
    public bool IS_ACTIVE { get; set; } = true;
}
