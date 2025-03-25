using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
public class TopicsDto
{


    [Column("TOPICS_ID")] // Matches DB column
    public int TopicsId { get; set; }

    [Column("CHAPTER_ID")] // Matches DB column
    public int ChapterId { get; set; }

    [Column("TOPICS_NAME")] // Matches DB column
    public string TopicsName { get; set; } = string.Empty;

    [Column("TOPIC_ATTACHMENT_FILE_NAME")] // Matches DB column
    public string Topic_attachment_file_Name { get; set; } = string.Empty;

    [Column("CLASS_ID")] // Matches DB column
    public int ClassId { get; set; }

    [Column("SUBJECT_ID")] // Matches DB column
    public int SubjectId { get; set; }

    [Column("CHAPTER_NAME")] // Matches DB column
    public string ChapterName { get; set; } = string.Empty;

    [Column("COURSE_TYPE_ID")] // Matches DB column
    public int CourseTypeId { get; set; }

    [Column("TOPICS_DESCRIPTION")] // Matches DB column
    public string TopicsDescription { get; set; } = string.Empty;

    [Column("IS_ACTIVE")] // Matches DB column
    public bool IsActive { get; set; } // Ensure correct data type

    [Column("TOTALCOUNT")] // Matches DB column
    public int TotalCount { get; set; }

    public class TopicsAttactmentsDTO
    {


        [Column("TOPIC_ATTACHMENTS_ID")] // Matches DB column
        public int topics_Attachments_Id { get; set; }

        [Column("TOPICS_ID")] // Matches DB column
        public int TopicsId { get; set; }

        [Column("CHAPTER_ID")] // Matches DB column
        public int ChapterId { get; set; }

        [Column("TOPIC_ATTACHMENT_FILE_NAME")] // Matches DB column
        public string topics_Attachments_FileName { get; set; } = string.Empty;

        [Column("SUBJECT_ID")] // Matches DB column
        public int SubjectId { get; set; }

        [Column("IS_ACTIVE")] // Matches DB column
        public bool IsActive { get; set; } // Ensure correct data type

    }


    // Save OR UPDATE TOPIC Data
    public class SaveTopicDto
    {
        public int TopicsId { get; set; } // Matches CHAPTER_ID in DB
        public int ChapterId { get; set; } // Matches CHAPTER_ID in DB

        [Required]
        public string TopicsName { get; set; } = string.Empty; // Matches CHAPTER_NAME
        public string TopicsDescription { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true; // Matches IS_ACTIVE

        public List<IFormFile>? Files { get; set; }


        public List<string>? FilePaths { get; set; }

        public int UserId { get; set; }



    }

    public class TopicsBySubID
    {


        [Column("TOPICS_ID")] // Matches DB column
        public int TopicsId { get; set; }

        [Column("CHAPTER_ID")] // Matches DB column
        public int ChapterId { get; set; }

        [Column("TOPICS_NAME")] // Matches DB column
        public string TopicsName { get; set; } = string.Empty;
    }


}

