using Microsoft.EntityFrameworkCore;
using Project.Api.Data;
using Project.Api.Data.Interfaces;
using Project.Api.Data.Repositories;

// Criar ConnectionContext
// Criar Interfaces de contratos
// Criar um repositório e herdar da Interface correspondente


namespace Project.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers / Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Registra o DbContext com Npgsql
            builder.Services.AddDbContext<ConnectionContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

            var app = builder.Build();       

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Permitir servir arquivos de wwwroot
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
