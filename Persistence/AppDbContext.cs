using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<ActorShow> Cast { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActorShow>()
                .HasKey(e => new { e.ActorId, e.ShowId });
        }
    }
}