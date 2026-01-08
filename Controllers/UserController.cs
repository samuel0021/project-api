using Microsoft.AspNetCore.Mvc;
using Project.Api.Data.Interfaces;
using Project.Api.DTO.Users;
using Project.Api.Model;
using Project.Api.Models;

namespace Project.Api.Controllers
{
    // Mudar a herança pra ControlleBase
    // Definir ApiController e Route
    // Declarar campo privado da Interface correspondente
    // Definir os métodos HTTP

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET api/users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _repository.GetAll()
                .Select(u => new UserDetailsDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Age = u.Age,
                    Email = u.Email
                });

            return Ok(users);
        }

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var user = _repository.GetById(id);

            if (user == null)
                return NotFound();

            var dto = new UserDetailsDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = $"{user.FirstName} {user.LastName}",
                Age = user.Age,
                Email = user.Email
            };

            return Ok(dto);
        }

        // POST api/users
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(dto);

            var user = new User(
                dto.FirstName,
                dto.LastName,
                dto.Age,
                dto.Email
                );

            _repository.Add(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // PATCH api/users
        [HttpPatch("{id:int}")]
        public IActionResult UpdateUser([FromBody] UserUpdateDto dto, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(dto);

            var user = _repository.GetById(id);

            if (user == null)
                return NotFound();

            if (dto.FirstName != null)
            {
                user.FirstName = dto.FirstName;
            }
            if (dto.LastName != null)
            {
                user.LastName = dto.LastName;
            }
            if (dto.Age != null)
            {
                user.Age = dto.Age.Value;
            }
            if (dto.Email != null)
            {
                user.Email = dto.Email;
            }

            _repository.Update(user);

            return Ok(user);
        }

        // DELETE api/users
        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _repository.GetById(id);

            if (user == null)
                return NotFound();

            _repository.Delete(user);

            return Ok(user);
        }
    }
}
