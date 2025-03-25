using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSurePhysicsWebAPI.Models
{
    [Table("USERS")]
    public class UserRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // SP generates ID manually
        [Column("USERS_ID")]
        public int UsersId { get; set; }

        [Required, StringLength(20)]
        [Column("FIRST_NAME")]
        public required string FirstName { get; set; }

        [Required, StringLength(20)]
        [Column("LAST_NAME")]
        public required string LastName { get; set; }

        [Required, EmailAddress, StringLength(255)]
        [Column("EMAIL")]
        public required string Email { get; set; }

        [Required, StringLength(20, MinimumLength = 6)]
        [Column("PASSWORD_MAIL")]
        public required string Password { get; set; }

        [Column("PHONE_NUMBER")]
        public long? PhoneNumber { get; set; } // Changed to long? for BIGINT

        [StringLength(255)]
        [Column("ADDRESS")]
        public string? Address { get; set; }

        [StringLength(20)]
        [Column("CITY")]
        public string? City { get; set; }

        [StringLength(20)]
        [Column("STATE")]
        public string? State { get; set; }

        [Column("CLASS_ID")]
        public int ClassId { get; set; }

        [StringLength(50)] // Matches SP's VARCHAR(50)
        [Column("IMAGE")] // Maps to IMAGE column
        public string? Image { get; set; } // Added for IMAGE_PATH

        [Column("ROLE_ID")]
        public int RoleId { get; set; } = 2; // Updated default to match SP

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        [Column("CREATED_BY")]
        public int CreatedBy { get; set; } // Required, removed nullable

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("MODIFIED_BY")]
        public int ModifiedBy { get; set; } = 1;

        [Column("MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}