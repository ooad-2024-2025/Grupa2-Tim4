using Aplikacija.Data;
using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class TreningController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public TreningController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Prikaz treninga - za trenere i administratore
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Index(string trenerId = null)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            IQueryable<Trening> treninzi = _context.Trening
                .Include(t => t.Clan)
                .Include(t => t.Trener);

            // Ako je administrator i specifičan trener je odabran
            if (isAdmin && !string.IsNullOrEmpty(trenerId))
            {
                treninzi = treninzi.Where(t => t.TrenerId == trenerId);
                ViewBag.SelectedTrenerId = trenerId;
            }
            // Ako je obični trener, prikaži samo njegove treninge
            else if (!isAdmin)
            {
                treninzi = treninzi.Where(t => t.TrenerId == korisnik.Id);
            }
            // Ako je administrator bez odabranog trenera, prikaži sve treninge

            // Dropdown lista trenera (samo za administratore)
            if (isAdmin)
            {
                var treneri = await _context.Korisnik
                    .Where(k => k.Tip == TipKorisnika.Trener)
                    .Select(k => new { k.Id, k.Email })
                    .ToListAsync();

                ViewBag.Treneri = new SelectList(treneri, "Id", "Email", trenerId);
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(await treninzi.ToListAsync());
        }

        // Detalji treninga - dostupno oboma
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var trening = await _context.Trening
                .Include(t => t.Clan)
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(m => m.IdTrening == id);

            if (trening == null)
                return NotFound();

            // Provjeri da li trener može pristupiti samo svojim treningu
            if (!isAdmin && trening.TrenerId != korisnik.Id)
                return Forbid();

            return View(trening);
        }

        // Kreiranje treninga - samo treneri i administratori
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Create()
        {
            var clanovi = _context.Korisnik
                .Where(k => k.Tip == TipKorisnika.Clan)
                .Select(k => new { k.Id, k.Email })
                .ToList();

            ViewData["ClanId"] = new SelectList(clanovi, "Id", "Email");

            // Administrator može odabrati trenera
            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admini");

            if (isAdmin)
            {
                var treneri = _context.Korisnik
                    .Where(k => k.Tip == TipKorisnika.Trener)
                    .Select(k => new { k.Id, k.Email })
                    .ToList();
                ViewData["TrenerId"] = new SelectList(treneri, "Id", "Email");
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Create([Bind("Datum,Vrijeme,Tip,ClanId,TrenerId")] Trening trening)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            // Ako nije admin, automatski postavi trenera na trenutnog korisnika
            if (!isAdmin)
            {
                trening.TrenerId = korisnik.Id;
            }

            if (ModelState.IsValid)
            {
                _context.Add(trening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Vraćanje dropdown lista u slučaju greške
            var clanovi = _context.Korisnik
                .Where(k => k.Tip == TipKorisnika.Clan)
                .Select(k => new { k.Id, k.Email })
                .ToList();
            ViewData["ClanId"] = new SelectList(clanovi, "Id", "Email", trening.ClanId);

            if (isAdmin)
            {
                var treneri = _context.Korisnik
                    .Where(k => k.Tip == TipKorisnika.Trener)
                    .Select(k => new { k.Id, k.Email })
                    .ToList();
                ViewData["TrenerId"] = new SelectList(treneri, "Id", "Email", trening.TrenerId);
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(trening);
        }

        // Edit treninga - samo treneri i administratori
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var trening = await _context.Trening.FindAsync(id);
            if (trening == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            // Provjeri da li trener može uređivati samo svoj trening
            if (!isAdmin && trening.TrenerId != korisnik.Id)
                return Forbid();

            ViewData["ClanId"] = new SelectList(_context.Korisnik.Where(k => k.Tip == TipKorisnika.Clan), "Id", "Email", trening.ClanId);

            if (isAdmin)
            {
                ViewData["TrenerId"] = new SelectList(_context.Korisnik.Where(k => k.Tip == TipKorisnika.Trener), "Id", "Email", trening.TrenerId);
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(trening);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("IdTrening,Datum,Vrijeme,Tip,ClanId,TrenerId")] Trening trening)
        {
            if (id != trening.IdTrening)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            // Provjeri dozvole
            var originalTrening = await _context.Trening.AsNoTracking().FirstOrDefaultAsync(t => t.IdTrening == id);
            if (!isAdmin && originalTrening.TrenerId != korisnik.Id)
                return Forbid();

            // Ako nije admin, ne dozvoli mijenjanje trenera
            if (!isAdmin)
            {
                trening.TrenerId = korisnik.Id;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreningExists(trening.IdTrening))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClanId"] = new SelectList(_context.Korisnik.Where(k => k.Tip == TipKorisnika.Clan), "Id", "Email", trening.ClanId);

            if (isAdmin)
            {
                ViewData["TrenerId"] = new SelectList(_context.Korisnik.Where(k => k.Tip == TipKorisnika.Trener), "Id", "Email", trening.TrenerId);
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(trening);
        }

        // Brisanje treninga - samo treneri i administratori
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var trening = await _context.Trening
                .Include(t => t.Clan)
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(m => m.IdTrening == id);

            if (trening == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            // Provjeri da li trener može brisati samo svoj trening
            if (!isAdmin && trening.TrenerId != korisnik.Id)
                return Forbid();

            return View(trening);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trening = await _context.Trening.FindAsync(id);
            if (trening != null)
            {
                var korisnik = await _userManager.GetUserAsync(User);
                var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

                // Provjeri dozvole
                if (!isAdmin && trening.TrenerId != korisnik.Id)
                    return Forbid();

                _context.Trening.Remove(trening);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TreningExists(int id)
        {
            return _context.Trening.Any(e => e.IdTrening == id);
        }
    }
}