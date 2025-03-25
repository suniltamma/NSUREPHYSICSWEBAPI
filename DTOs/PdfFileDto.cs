using System;
using System.ComponentModel.DataAnnotations.Schema;

public class PdfFileDto
{
    [Column("TOPIC_ATTACHMENTS_ID")]
    public int TopicAttachmentsId { get; set; }

    [Column("TOPICS_ID")]
    public int TopicsId { get; set; }

    [Column("CHAPTER_ID")]
    public int ChapterId { get; set; }

    [Column("CHAPTER_NAME")]
    public string? ChapterName { get; set; }

    [Column("SUBJECT_ID")]
    public int SubjectId { get; set; }

    [Column("SUBJECT_NAME")]
    public string? SubjectName { get; set; }

    [Column("TOPIC_ATTACHMENT_FILE_NAME")]
    public string? TopicAttachmentFileName { get; set; }

    [Column("CREATED_BY")]
    public int CreatedBy { get; set; }

    [Column("CREATED_DATE")]
    public DateTime CreatedDate { get; set; }

    [Column("IS_ACTIVE")]
    public bool IsActive { get; set; }
}
