using System.ComponentModel.DataAnnotations;

namespace Project.Api.DTO.Reviews
{
    public class ReviewCreateDto
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]        
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public IFormFile? Image { get; set; } // opcional

    }
}
