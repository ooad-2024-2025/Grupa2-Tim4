﻿// Controllers/TerminController.cs - AŽURIRANI (samo ključne izmene)
using Aplikacija.Data;
using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class TerminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public TerminController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Termin - Za trenere i administratore (OSTAJE ISTO)
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Index()
        {
            var termini = await _context.Termin
                .Include(t => t.Trener)
                .ToListAsync();

            return View(termini);
        }

        // GET: DostupniTermini - Za korisnike/članove i recepcionere (OSTAJE ISTO)
        [Authorize(Roles = "Korisnik,Recepcioner")]
        public async Task<IActionResult> DostupniTermini()
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            var termini = await _context.Termin
                .Include(t => t.Trener)
                .Include(t => t.Prijave)
                .Where(t => t.Datum >= DateTime.Now.Date)
                .OrderBy(t => t.Datum)
                .ThenBy(t => t.Vrijeme)
                .ToListAsync();

            ViewBag.KorisnikId = korisnik.Id;
            ViewBag.IsRecepcioner = await _userManager.IsInRoleAsync(korisnik, "Recepcioner");

            return View(termini);
        }

        // GET: Termin/Details/5 (OSTAJE ISTO)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var termin = await _context.Termin
                .Include(t => t.Trener)
                .Include(t => t.Prijave)
                .ThenInclude(p => p.Clan)
                .FirstOrDefaultAsync(t => t.IdTermin == id);

            if (termin == null)
                return NotFound();

            return View(termin);
        }

        // GET: Termin/Create (OSTAJE ISTO)
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Create()
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            var termin = new Termin
            {
                Datum = DateTime.Today.AddDays(1),
                Vrijeme = new TimeOnly(9, 0),
                Vrsta = VrstaTreninga.Grupni
            };

            bool jeTrener = await _userManager.IsInRoleAsync(korisnik, "Trener");
            bool jeAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            if (jeTrener && !jeAdmin)
            {
                termin.TrenerId = korisnik.Id;
                ViewBag.JeTrener = true;
                ViewBag.JeAdmin = false;
            }
            else if (jeAdmin)
            {
                ViewBag.JeTrener = false;
                ViewBag.JeAdmin = true;

                var treneri = await _context.Korisnik
                    .Where(k => k.Tip == TipKorisnika.Trener)
                    .Select(k => new { k.Id, k.Email })
                    .ToListAsync();

                ViewBag.TrenerId = new SelectList(treneri, "Id", "Email");
            }

            return View(termin);
        }

        // POST: Termin/Create (OSTAJE ISTO)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Create([Bind("Datum,Vrijeme,Vrsta,TrenerId")] Termin termin)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            bool jeTrener = await _userManager.IsInRoleAsync(korisnik, "Trener");
            bool jeAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            if (jeTrener && !jeAdmin)
            {
                termin.TrenerId = korisnik.Id;
            }
            else if (jeAdmin)
            {
                if (string.IsNullOrEmpty(termin.TrenerId))
                {
                    ModelState.AddModelError("TrenerId", "Morate izabrati trenera.");
                }
                else
                {
                    var trenerPostoji = await _context.Korisnik
                        .AnyAsync(k => k.Id == termin.TrenerId && k.Tip == TipKorisnika.Trener);

                    if (!trenerPostoji)
                    {
                        ModelState.AddModelError("TrenerId", "Izabrani trener ne postoji.");
                    }
                }
            }

            if (termin.Datum.Date < DateTime.Today)
            {
                ModelState.AddModelError("Datum", "Datum ne može biti u prošlosti.");
            }

            ModelState.Remove("Trener");
            ModelState.Remove("Prijave");
            ModelState.Remove("RealizovaniTreninzi"); // NOVO

            var postojeciTermin = await _context.Termin
                .FirstOrDefaultAsync(t => t.Datum.Date == termin.Datum.Date
                                       && t.Vrijeme == termin.Vrijeme
                                       && t.TrenerId == termin.TrenerId);

            if (postojeciTermin != null)
            {
                ModelState.AddModelError("", "Termin sa istim datumom i vremenom već postoji za ovog trenera.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Termin.Add(termin);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                    {
                        TempData["SuccessMessage"] = "Termin je uspješno kreiran.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Termin nije sačuvan u bazu podataka.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"EXCEPTION pri čuvanju termina: {ex.Message}");
                    ModelState.AddModelError("", "Greška pri kreiranju termina. Molimo pokušajte ponovo.");
                }
            }

            // Povrati ViewBag vrijednosti u slučaju greške
            if (jeTrener && !jeAdmin)
            {
                ViewBag.JeTrener = true;
                ViewBag.JeAdmin = false;
            }
            else if (jeAdmin)
            {
                ViewBag.JeTrener = false;
                ViewBag.JeAdmin = true;

                var treneri = await _context.Korisnik
                    .Where(k => k.Tip == TipKorisnika.Trener)
                    .Select(k => new { k.Id, k.Email })
                    .ToListAsync();

                ViewBag.TrenerId = new SelectList(treneri, "Id", "Email", termin.TrenerId);
            }

            return View(termin);
        }

        // POST: Rezervisi termin - KLJUČNA IZMENA!
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Korisnik")]
        public async Task<IActionResult> Rezervisi(int terminId)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            bool vecPrijavljen = await _context.PrijaveNaTermine
                .AnyAsync(p => p.TerminId == terminId && p.ClanId == korisnik.Id);
            if (vecPrijavljen)
            {
                TempData["ErrorMessage"] = "Već ste prijavljeni na ovaj termin.";
                return RedirectToAction(nameof(DostupniTermini));
            }

            var termin = await _context.Termin
                .Include(t => t.Prijave)
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(t => t.IdTermin == terminId);
            if (termin == null)
            {
                TempData["ErrorMessage"] = "Termin nije pronađen.";
                return RedirectToAction(nameof(DostupniTermini));
            }

            var danas = DateTime.Now;
            var vremeTermina = termin.Datum.Add(termin.Vrijeme.ToTimeSpan());
            if (vremeTermina < danas)
            {
                TempData["ErrorMessage"] = "Ne možete se prijaviti na prošle termine.";
                return RedirectToAction(nameof(DostupniTermini));
            }

            if (termin.Vrsta == VrstaTreninga.Individualni)
            {
                bool individualnoPopunjeno = await _context.PrijaveNaTermine
                    .AnyAsync(p => p.TerminId == terminId);
                if (individualnoPopunjeno)
                {
                    TempData["ErrorMessage"] = "Ovaj individualni termin je već popunjen.";
                    return RedirectToAction(nameof(DostupniTermini));
                }
            }

            // Kreiranje prijave na termin (OSTAJE ISTO)
            var prijava = new PrijavaNaTermin
            {
                ClanId = korisnik.Id,
                TerminId = terminId,
            };
            _context.PrijaveNaTermine.Add(prijava);

            // IZMENA - kreiranje treninga koji referencira Termin umesto duplikovanja podataka
            var noviTrening = new Trening
            {
                TerminId = terminId,  // UMESTO duplikovanja Datum, Vrijeme, Tip, TrenerId
                ClanId = korisnik.Id,
                Status = StatusTreninga.Planiran,
                DatumKreiranja = DateTime.Now
            };
            _context.Trening.Add(noviTrening);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Uspješno ste se prijavili na termin.";
            return RedirectToAction(nameof(DostupniTermini));
        }

        // POST: Otkazi rezervaciju - KLJUČNA IZMENA!
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Korisnik")]
        public async Task<IActionResult> OtkaziRezervaciju(int terminId)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            var prijava = await _context.PrijaveNaTermine
                .FirstOrDefaultAsync(p => p.TerminId == terminId && p.ClanId == korisnik.Id);
            if (prijava == null)
            {
                TempData["ErrorMessage"] = "Niste prijavljeni na ovaj termin.";
                return RedirectToAction(nameof(DostupniTermini));
            }

            var termin = await _context.Termin.FindAsync(terminId);
            if (termin == null)
            {
                TempData["ErrorMessage"] = "Termin nije pronađen.";
                return RedirectToAction(nameof(DostupniTermini));
            }

            var vremeTermina = termin.Datum.Add(termin.Vrijeme.ToTimeSpan());
            if (vremeTermina <= DateTime.Now.AddHours(2))
            {
                TempData["ErrorMessage"] = "Ne možete otkazati rezervaciju manje od 2 sata prije termina.";
                return RedirectToAction(nameof(DostupniTermini));
            }

            // Ukloni prijavu iz tabele PrijaveNaTermine (OSTAJE ISTO)
            _context.PrijaveNaTermine.Remove(prijava);

            // IZMJENA - pronađi i ukloni trening na osnovu TerminId umesto složene logike
            var trening = await _context.Trening
                .FirstOrDefaultAsync(t => t.ClanId == korisnik.Id && t.TerminId == terminId);

            if (trening != null)
            {
                _context.Trening.Remove(trening);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Uspješno ste otkazali rezervaciju.";
            return RedirectToAction(nameof(DostupniTermini));
        }

        // Ostale metode (MojiTermini, Edit, Delete) OSTAJU ISTE kao u originalnom kodu
        [Authorize(Roles = "Korisnik")]
        public async Task<IActionResult> MojiTermini()
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            var mojiTermini = await _context.PrijaveNaTermine
                .Include(p => p.Termin)
                .ThenInclude(t => t.Trener)
                .Where(p => p.ClanId == korisnik.Id)
                .OrderBy(p => p.Termin.Datum)
                .ThenBy(p => p.Termin.Vrijeme)
                .ToListAsync();

            return View(mojiTermini);
        }

        // Edit, Delete i ostale metode ostaju IDENTIČNE kao u originalnom kodu...
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var termin = await _context.Termin
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(t => t.IdTermin == id);

            if (termin == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            bool jeTrener = await _userManager.IsInRoleAsync(korisnik, "Trener");
            bool jeAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            if (jeTrener && !jeAdmin && termin.TrenerId != korisnik.Id)
            {
                TempData["ErrorMessage"] = "Ne možete editovati tuđe termine.";
                return RedirectToAction(nameof(Index));
            }

            if (jeTrener && !jeAdmin)
            {
                ViewBag.JeTrener = true;
                ViewBag.JeAdmin = false;
            }
            else if (jeAdmin)
            {
                ViewBag.JeTrener = false;
                ViewBag.JeAdmin = true;

                var treneri = await _context.Korisnik
                    .Where(k => k.Tip == TipKorisnika.Trener)
                    .Select(k => new { k.Id, k.Email })
                    .ToListAsync();

                ViewData["TrenerId"] = new SelectList(treneri, "Id", "Email", termin.TrenerId);
            }

            return View(termin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("IdTermin,Datum,Vrijeme,Vrsta,TrenerId")] Termin termin)
        {
            if (id != termin.IdTermin)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            bool jeTrener = await _userManager.IsInRoleAsync(korisnik, "Trener");
            bool jeAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var originalTermin = await _context.Termin.AsNoTracking().FirstOrDefaultAsync(t => t.IdTermin == id);
            if (originalTermin == null)
                return NotFound();

            if (jeTrener && !jeAdmin && originalTermin.TrenerId != korisnik.Id)
            {
                TempData["ErrorMessage"] = "Ne možete editovati tuđe termine.";
                return RedirectToAction(nameof(Index));
            }

            if (jeTrener && !jeAdmin)
            {
                termin.TrenerId = korisnik.Id;
            }
            else if (jeAdmin)
            {
                termin.TrenerId = originalTermin.TrenerId;
            }

            ModelState.Remove("Trener");
            ModelState.Remove("Prijave");
            ModelState.Remove("RealizovaniTreninzi"); // NOVO

            var postojeciTermin = await _context.Termin
                .FirstOrDefaultAsync(t => t.IdTermin != termin.IdTermin
                                       && t.Datum.Date == termin.Datum.Date
                                       && t.Vrijeme == termin.Vrijeme
                                       && t.TrenerId == termin.TrenerId);

            if (postojeciTermin != null)
            {
                ModelState.AddModelError("", "Termin sa istim datumom i vremenom već postoji za ovog trenera.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termin);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Termin je uspješno ažuriran.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminExists(termin.IdTermin))
                        return NotFound();
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Greška pri ažuriranju termina: {ex.Message}");
                    ModelState.AddModelError("", "Greška pri ažuriranju termina. Molimo pokušajte ponovo.");
                }
            }

            if (jeTrener && !jeAdmin)
            {
                ViewBag.JeTrener = true;
                ViewBag.JeAdmin = false;
            }
            else if (jeAdmin)
            {
                ViewBag.JeTrener = false;
                ViewBag.JeAdmin = true;

                var treneri = await _context.Korisnik
                    .Where(k => k.Tip == TipKorisnika.Trener)
                    .Select(k => new { k.Id, k.Email })
                    .ToListAsync();

                ViewData["TrenerId"] = new SelectList(treneri, "Id", "Email", termin.TrenerId);
            }

            return View(termin);
        }

        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var termin = await _context.Termin
                .Include(t => t.Trener)
                .Include(t => t.Prijave)
                .FirstOrDefaultAsync(t => t.IdTermin == id);

            if (termin == null)
                return NotFound();

            return View(termin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termin = await _context.Termin
                .Include(t => t.Prijave)
                .Include(t => t.RealizovaniTreninzi) // NOVO - uključi povezane treninge
                .FirstOrDefaultAsync(t => t.IdTermin == id);

            if (termin != null)
            {
                // Cascade delete će automatski obrisati povezane PrijaveNaTermine i Treninge
                if (termin.Prijave.Any())
                {
                    _context.PrijaveNaTermine.RemoveRange(termin.Prijave);
                }

                // Treninzi će se automatski obrisati zbog Cascade
                _context.Termin.Remove(termin);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Termin je uspješno uklonjen.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TerminExists(int id)
        {
            return _context.Termin.Any(e => e.IdTermin == id);
        }

        private async Task<bool> IsTerminAvailable(DateTime datum, TimeOnly vrijeme, string trenerId, int? excludeTerminId = null)
        {
            var query = _context.Termin
                .Where(t => t.Datum.Date == datum.Date
                         && t.Vrijeme == vrijeme
                         && t.TrenerId == trenerId);

            if (excludeTerminId.HasValue)
            {
                query = query.Where(t => t.IdTermin != excludeTerminId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}