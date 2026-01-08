using Project.Api.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Api.Models
{
    [Table("reviews")]
    public class Review
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Column("image_path")]
        public string? ImagePath { get; set; }  // Caminho/URL da imagem

        public Review() { }

        public Review(string title, string category, string description, int userId, User user, string? imagePath)
        {
            Title = title;
            Category = category;
            Description = description;
            UserId = userId;
            User = user;
            ImagePath = imagePath;
        }
    }
}
