using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Game.Models;
using Microsoft.AspNetCore.Identity;

namespace Game.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Character>? Characters { get; set; }
        public DbSet<Weapon>? Weapons { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder b) => b.LogTo(Console.WriteLine, LogLevel.Warning);
    }
}