using System.ComponentModel.DataAnnotations;

namespace Project.Api.DTO.Users
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [Range(18, 99)]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }
    }
}
