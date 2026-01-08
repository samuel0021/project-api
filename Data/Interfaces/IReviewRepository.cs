using Project.Api.Model;
using Project.Api.Models;

namespace Project.Api.Data.Interfaces
{
    public interface IReviewRepository
    {
        Review? GetById(int id);
        IEnumerable<Review> GetAll();
        void Add(Review review);
        void Update(Review review);
        void Delete(Review review);
    }
}
