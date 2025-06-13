// Data/ApplicationDbContext.cs - KOMPLETNO AŽURIRANI
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Aplikacija.Models;

namespace Aplikacija.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet definicije
        public DbSet<Aplikacija.Models.Korisnik> Korisnik { get; set; }
        public DbSet<Aplikacija.Models.Termin> Termin { get; set; }
        public DbSet<Aplikacija.Models.Trening> Trening { get; set; } = default!;
        public DbSet<Aplikacija.Models.Vezba> Vezbe { get; set; } = default!; // NOVO
        public DbSet<Aplikacija.Models.Kupovina> Kupovina { get; set; } = default!;
        public DbSet<Aplikacija.Models.PrijavaZaZaposljavanje> PrijavaZaZaposljavanje { get; set; } = default!;
        public DbSet<Aplikacija.Models.PlanIshrane> PlanIshrane { get; set; } = default!;
        public DbSet<PrijavaNaTermin> PrijaveNaTermine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tabele
            modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
            modelBuilder.Entity<Termin>().ToTable("Termin");
            modelBuilder.Entity<Trening>().ToTable("Trening");
            modelBuilder.Entity<Vezba>().ToTable("Vezba"); // NOVO
            modelBuilder.Entity<Kupovina>().ToTable("Kupovina");
            modelBuilder.Entity<PlanIshrane>().ToTable("PlanIshrane");
            modelBuilder.Entity<PrijavaZaZaposljavanje>().ToTable("PrijavaZaZaposljavanje");

            // NOVO - Trening -> Termin veza (umesto duplikovanja datuma/vremena)
            modelBuilder.Entity<Trening>()
                .HasOne(t => t.Termin)
                .WithMany(term => term.RealizovaniTreninzi)
                .HasForeignKey(t => t.TerminId)
                .OnDelete(DeleteBehavior.Cascade);

            // Trening -> Clan (ostaje isto)
            modelBuilder.Entity<Trening>()
                .HasOne(t => t.Clan)
                .WithMany(k => k.TreninziKaoClan)
                .HasForeignKey(t => t.ClanId)
                .OnDelete(DeleteBehavior.Cascade);

            // NOVO - Vezba -> Trening veza
            modelBuilder.Entity<Vezba>()
                .HasOne(v => v.Trening)
                .WithMany(t => t.Vezbe)
                .HasForeignKey(v => v.TreningId)
                .OnDelete(DeleteBehavior.Cascade);

            // Termin -> Trener (ostaje isto)
            modelBuilder.Entity<Termin>()
                .HasOne(t => t.Trener)
                .WithMany(k => k.TerminiTrenera)
                .HasForeignKey(t => t.TrenerId)
                .OnDelete(DeleteBehavior.Restrict);

            // PlanIshrane -> Korisnik (ostaje isto)
            modelBuilder.Entity<PlanIshrane>()
                .HasOne(p => p.Clan)
                .WithOne(k => k.Plan_Ishrane)
                .HasForeignKey<PlanIshrane>(p => p.ClanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prijava -> Korisnik (ostaje isto)
            modelBuilder.Entity<PrijavaZaZaposljavanje>()
                .HasOne(p => p.Korisnik)
                .WithMany(k => k.Prijave)
                .HasForeignKey(p => p.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            // PrijavaNaTermin veze (dodaj ako nisu već definisane)
            modelBuilder.Entity<PrijavaNaTermin>()
                .HasOne(p => p.Clan)
                .WithMany()
                .HasForeignKey(p => p.ClanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrijavaNaTermin>()
                .HasOne(p => p.Termin)
                .WithMany(t => t.Prijave)
                .HasForeignKey(p => p.TerminId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indeksi za performance
            modelBuilder.Entity<Trening>()
                .HasIndex(t => t.TerminId);

            modelBuilder.Entity<Trening>()
                .HasIndex(t => t.ClanId);

            modelBuilder.Entity<Vezba>()
                .HasIndex(v => v.TreningId);

            modelBuilder.Entity<Vezba>()
                .HasIndex(v => new { v.TreningId, v.Redosled })
                .IsUnique();
        }
    }
}