using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Aplikacija.Models;

namespace Aplikacija.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aplikacija.Models.Korisnik> Korisnik { get; set; }
        public DbSet<Aplikacija.Models.Termin> Termin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aplikacija.Models.Korisnik>().ToTable("Korisnik");
            modelBuilder.Entity<Aplikacija.Models.Termin>().ToTable("Termin");

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Aplikacija.Models.Kupovina> Kupovina { get; set; } = default!;
        public DbSet<Aplikacija.Models.PrijavaZaZaposljavanje> PrijavaZaZaposljavanje { get; set; } = default!;
        public DbSet<Aplikacija.Models.PlanIshrane> PlanIshrane { get; set; } = default!;

    }
}
