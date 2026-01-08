using Microsoft.EntityFrameworkCore;
using Project.Api.Data.Interfaces;
using Project.Api.Models;

namespace Project.Api.Data.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ConnectionContext _context;

        public ReviewRepository(ConnectionContext context)
        {
            _context = context;
        }

        public Review? GetById(int id)
        {
            return _context.Reviews
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Review> GetAll()
        {
            return _context.Reviews.Include(r => r.User);
        }

        public void Add(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public void Update(Review review)
        {
            _context.Reviews.Update(review);
            _context.SaveChanges();            
        }

        public void Delete(Review review)
        {
            _context.Reviews.Remove(review);
            _context.SaveChanges();                        
        }

    }
}
