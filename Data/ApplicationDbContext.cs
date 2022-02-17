using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Game.Models;

namespace Game.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Character>? Characters { get; set; }
        public DbSet<Weapon>? Weapons { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder b) => b.LogTo(Console.WriteLine, LogLevel.Warning);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // When a weapon is deleted, do not delete characters with that weapon but set the foreign key to null
            modelBuilder.Entity<Character>()
                .HasOne(p => p.Weapon)
                .WithMany(w => w.Characters)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}