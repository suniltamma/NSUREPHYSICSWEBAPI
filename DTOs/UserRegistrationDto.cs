using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NSurePhysicsWebAPI.DTOs
{
    public class UserRegistrationDto
    {
        [Required, StringLength(20)]
        public required string FirstName { get; set; }

        [Required, StringLength(20)]
        public required string LastName { get; set; }

        [Required, EmailAddress, StringLength(255)]
        public required string Email { get; set; }

        [Required, StringLength(20, MinimumLength = 6)]
        public required string Password { get; set; }

        public long? PhoneNumber { get; set; } // Changed to long? for BIGINT

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? City { get; set; }

        [StringLength(20)]
        public string? State { get; set; }

        public int ClassId { get; set; }

        public IFormFile? Image { get; set; }

        [StringLength(50)]
        public string? ImagePath { get; set; }

        public int CreatedBy { get; set; } // Added, required by SP
    }
}