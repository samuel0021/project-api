using Microsoft.EntityFrameworkCore;
using Project.Api.Model;
using Project.Api.Models;

namespace Project.Api.Data
{
    // Herdar de DbContext e gerar o construtor
    // Sempre criar o DbSet dos models aqui
    public class ConnectionContext : DbContext
    {
        public ConnectionContext(DbContextOptions options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
