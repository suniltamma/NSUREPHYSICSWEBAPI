using System.ComponentModel.DataAnnotations.Schema;

namespace NSurePhysicsWebAPI.DTOs
{
    public class VideoFilesDto
    {
        [Column("VIDEO_ID")]
        public int VideoId { get; set; }

        [Column("CHAPTER_ID")]
        public int? ChapterId { get; set; }

        [Column("VIDEO_DESC")]
        public string VideoDesc { get; set; } = string.Empty;

        [Column("FILE_PATH")]
        public string FilePath { get; set; } = string.Empty;

        [Column("YOUTUBE_PATH")]
        public string YoutubePath { get; set; } = string.Empty;

        [Column("VIDEO_TYPE")]
        public int VideoType { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
    }
}