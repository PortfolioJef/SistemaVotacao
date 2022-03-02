using IMDbApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IMDbApi.Context
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {

        }
        public DbSet<Actor> actors { get; set; }
        public DbSet<Movie> movies { get; set; }
        public DbSet<MovieGenre> movieGenres { get; set; }
        public DbSet<Rating> ratings { get; set; }
        public DbSet<User> users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Email = "admin@empresa.net",
                        isAdmin = true,
                        isDeleted = false,
                        Lastname = "admin",
                        Name = "admin",
                        //Senha 123
                        Password = "UUVHGRBjB7w="
                    }
                );
        }
    }
}



