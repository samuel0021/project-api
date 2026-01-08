using System.ComponentModel.DataAnnotations;

namespace Project.Api.DTO.Users
{
    public class UserUpdateDto
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        
        [MaxLength(100)]
        public string? LastName { get; set; }

        [Range(18, 99)]
        public int? Age { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }
    }
}
