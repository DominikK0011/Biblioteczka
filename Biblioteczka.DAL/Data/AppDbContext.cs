using Biblioteczka.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Biblioteczka.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public AppDbContext() { }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.MovieDirector)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.MovieActor);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ParentReview)
                .WithMany(parentReview => parentReview.Replies)
                .HasForeignKey(r => r.ParentReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Person)
                .WithMany()
                .HasForeignKey(u => u.PersonId)
                .IsRequired(false);
        }
    }
}
