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
        public DbSet<Aplikacija.Models.Trening> Trening { get; set; } = default!;
        public DbSet<Aplikacija.Models.Kupovina> Kupovina { get; set; } = default!;
        public DbSet<Aplikacija.Models.PrijavaZaZaposljavanje> PrijavaZaZaposljavanje { get; set; } = default!;
        public DbSet<Aplikacija.Models.PlanIshrane> PlanIshrane { get; set; } = default!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tabele
            modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
            modelBuilder.Entity<Termin>().ToTable("Termin");
            modelBuilder.Entity<Kupovina>().ToTable("Kupovina");
            modelBuilder.Entity<PlanIshrane>().ToTable("PlanIshrane");
            modelBuilder.Entity<PrijavaZaZaposljavanje>().ToTable("PrijavaZaZaposljavanje");
            modelBuilder.Entity<Trening>().ToTable("Trening");


            // Trening -> Clan
            modelBuilder.Entity<Trening>()
                .HasOne(t => t.Clan)
                .WithMany(k => k.TreninziKaoClan)
                .HasForeignKey(t => t.ClanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Trening -> Trener
            modelBuilder.Entity<Trening>()
                .HasOne(t => t.Trener)
                .WithMany(k => k.TreninziKaoTrener)
                .HasForeignKey(t => t.TrenerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Termin -> Trener (jedan korisnik moze imati vise termina kao trener)
            modelBuilder.Entity<Termin>()
                .HasOne(t => t.Trener)
                .WithMany(k => k.TerminiTrenera)
                .HasForeignKey(t => t.TrenerId)
                .OnDelete(DeleteBehavior.Restrict);

            // PlanIshrane -> Korisnik (jedan plan je vezan za jednog korisnika)
            modelBuilder.Entity<PlanIshrane>()
                .HasOne(p => p.Clan)
                .WithOne(k => k.Plan_Ishrane)
                .HasForeignKey<PlanIshrane>(p => p.ClanId)
                .OnDelete(DeleteBehavior.Cascade);


            // Prijava -> Korisnik (više prijava po korisniku)
            modelBuilder.Entity<PrijavaZaZaposljavanje>()
                .HasOne(p => p.Korisnik)
                .WithMany(k => k.Prijave)
                .HasForeignKey(p => p.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }


    }
}
