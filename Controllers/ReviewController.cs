using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Data;
using Project.Api.Data.Interfaces;
using Project.Api.DTO.Reviews;
using Project.Api.DTO.Users;
using Project.Api.Model;
using Project.Api.Models;

namespace Project.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _repository;
        private readonly ConnectionContext _context; // pra validar se o user existe
        private readonly IWebHostEnvironment _env;


        public ReviewController(IReviewRepository repository, ConnectionContext context, IWebHostEnvironment env)
        {
            _repository = repository;
            _context = context;
            _env = env;
        }

        // GET api/reviews/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetReviewById(int id)
        {
            var review = _repository.GetById(id);

            if (review == null)
                return NotFound();

            var dto = new ReviewDetailsDto
            {
                Id = review.Id,
                Title = review.Title,
                Category = review.Category,
                Description = review.Description,
                UserId = review.UserId,
                UserName = $"{review.User.FirstName} {review.User.LastName}",
                ImageUrl = review.ImagePath

            };

            return Ok(dto);
        }

        // GET api/reviews
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _repository.GetAll()
                .Select(r => new ReviewDetailsDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Category = r.Category,
                    Description = r.Description,
                    UserId = r.UserId,
                    UserName = $"{r.User.FirstName} {r.User.LastName}",
                    ImageUrl = r.ImagePath
                });

            return Ok(reviews);
        }

        // POST api/reviews
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult CreateReview([FromForm] ReviewCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = _context.Users.Any(u => u.Id == dto.UserId);
            if (!userExists)
                return NotFound($"User with id {dto.UserId} not found.");

            string? imagePath = null;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "reviews", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.Image.CopyTo(stream);
                }

                // caminho relativo para o cliente
                imagePath = $"/reviews/uploads/{fileName}";
            }

            var review = new Review
            {
                Title = dto.Title,
                Category = dto.Category,
                Description = dto.Description,
                UserId = dto.UserId,
                ImagePath = imagePath
            };

            _repository.Add(review);

            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
        }

        // PATCH api/reviews/{id}
        [HttpPatch("{id:int}")]
        [Consumes("multipart/form-data")]
        public IActionResult UpdateReview([FromForm] ReviewUpdateDto dto, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = _repository.GetById(id);
            if (review == null)
                return NotFound();

            if (dto.UserId != null)
            {
                var userExists = _context.Users.Any(u => u.Id == dto.UserId.Value);
                if (!userExists)
                    return NotFound($"User with id {dto.UserId} not found.");

                review.UserId = dto.UserId.Value;
            }

            if (dto.Title != null)
                review.Title = dto.Title;
            
            if (dto.Category!= null)
                review.Category = dto.Category;

            if (dto.Description != null)
                review.Description = dto.Description;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                // opcional: deletar imagem antiga se existir
                if (!string.IsNullOrEmpty(review.ImagePath))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, review.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "reviews", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.Image.CopyTo(stream);
                }

                review.ImagePath = $"/reviews/uploads/{fileName}";
            }

            _repository.Update(review);

            return NoContent();
        }

        // DELETE api/reviews/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteReview(int id)
        {
            var review = _repository.GetById(id);
            if (review == null)
                return NotFound();

            // opcional: apagar imagem associada
            if (!string.IsNullOrEmpty(review.ImagePath))
            {
                var oldPath = Path.Combine(_env.WebRootPath, review.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            _repository.Delete(review);

            return NoContent();
        }
    }
}
