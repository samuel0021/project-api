using Project.Api.Model;

namespace Project.Api.Data.Interfaces
{
    // Contratos de operações dos modelos (add, list, delete, etc...)
    public interface IUserRepository
    {
        User? GetById(int id);
        IEnumerable<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
