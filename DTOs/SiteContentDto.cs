// NSurePhysicsWebAPI/DTOs/SiteContentDto.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace NSurePhysicsWebAPI.DTOs
{
    public class SiteContentDto
    {
        [Column("Content_ID")]
        public int ContentId { get; set; }

        [Column("Content_Name")]
        public string ContentName { get; set; } = string.Empty;

        [Column("Content_Desc")]
        public string ContentDesc { get; set; } = string.Empty;

        [Column("Content_Image_Url")]
        public string ContentImageUrl { get; set; } = string.Empty;

        [Column("Image_Width")]
        public int ImageWidth { get; set; }

        [Column("Image_Height")]
        public int ImageHeight { get; set; }

        [Column("Is_Active")]
        public bool IsActive { get; set; }
    }


    public class PhotoGalleryDto
    {
        [Column("PHOTO_ID")]
        public int PhotoId { get; set; }

        [Column("PHOTO_NAME")]
        public string PhotoName { get; set; } = string.Empty;

        [Column("PHOTO_URL")]
        public string PhotoUrl { get; set; } = string.Empty;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
    }
}