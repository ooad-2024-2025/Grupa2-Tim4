// Controllers/TreningController.cs - ISPRAVLJEN SA STANDARDNIM NAZIVIMA
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

        // GET: Prikaz treninga - za trenere i administratore
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Index(string trenerId = null, StatusTreninga? status = null)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            IQueryable<Trening> treninzi = _context.Trening
                .Include(t => t.Termin)
                .ThenInclude(term => term.Trener)
                .Include(t => t.Clan)
                .Include(t => t.Vezbe);

            // Filtriranje po treneru
            if (isAdmin && !string.IsNullOrEmpty(trenerId))
            {
                treninzi = treninzi.Where(t => t.Termin.TrenerId == trenerId);
                ViewBag.SelectedTrenerId = trenerId;
            }
            else if (!isAdmin)
            {
                treninzi = treninzi.Where(t => t.Termin.TrenerId == korisnik.Id);
            }

            // Filtriranje po statusu
            if (status.HasValue)
            {
                treninzi = treninzi.Where(t => t.Status == status.Value);
                ViewBag.SelectedStatus = status.Value;
            }

            // Sortiranje po datumu termina
            treninzi = treninzi.OrderByDescending(t => t.Termin.Datum).ThenByDescending(t => t.Termin.Vrijeme);

            // Dropdown liste za filtriranje (samo za administratore)
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

            // Dropdown za status
            ViewBag.StatusOptions = new SelectList(Enum.GetValues(typeof(StatusTreninga))
                .Cast<StatusTreninga>()
                .Select(s => new { Value = s, Text = s.ToString() }), "Value", "Text", status);

            return View(await treninzi.ToListAsync());
        }

        // GET: Detalji treninga
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var trening = await _context.Trening
                .Include(t => t.Termin)
                .ThenInclude(term => term.Trener)
                .Include(t => t.Clan)
                .Include(t => t.Vezbe.OrderBy(v => v.Redosled))
                .FirstOrDefaultAsync(m => m.IdTrening == id);

            if (trening == null)
                return NotFound();

            // Provjeri da li trener može pristupiti samo svojim treninzima
            if (!isAdmin && trening.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            return View(trening);
        }

        // GET: Upravljanje statusom treninga
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> UpravljajStatusom(int? id)
        {
            if (id == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var trening = await _context.Trening
                .Include(t => t.Termin)
                .ThenInclude(term => term.Trener)
                .Include(t => t.Clan)
                .Include(t => t.Vezbe.OrderBy(v => v.Redosled))
                .FirstOrDefaultAsync(m => m.IdTrening == id);

            if (trening == null)
                return NotFound();

            if (!isAdmin && trening.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            ViewBag.StatusOptions = new SelectList(Enum.GetValues(typeof(StatusTreninga))
                .Cast<StatusTreninga>(), trening.Status);

            return View(trening);
        }

        // POST: Ažuriraj status treninga
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> UpravljajStatusom(int id, StatusTreninga status, string planTreninga, string napomene)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var trening = await _context.Trening
                .Include(t => t.Termin)
                .FirstOrDefaultAsync(t => t.IdTrening == id);

            if (trening == null)
                return NotFound();

            if (!isAdmin && trening.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            trening.Status = status;
            trening.PlanTreninga = planTreninga;
            trening.Napomene = napomene;
            trening.PoslednaIzmena = DateTime.Now;

            try
            {
                _context.Update(trening);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Status treninga je uspješno ažuriran.";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Greška pri ažuriranju statusa: {ex.Message}");
                ModelState.AddModelError("", "Greška pri ažuriranju statusa treninga.");

                ViewBag.StatusOptions = new SelectList(Enum.GetValues(typeof(StatusTreninga))
                    .Cast<StatusTreninga>(), status);

                return View(await _context.Trening
                    .Include(t => t.Termin)
                    .ThenInclude(term => term.Trener)
                    .Include(t => t.Clan)
                    .Include(t => t.Vezbe)
                    .FirstOrDefaultAsync(t => t.IdTrening == id));
            }
        }

        // GET: Create vežbu (umesto DodajVezbu)
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Create(int? treningId)
        {
            if (treningId == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var trening = await _context.Trening
                .Include(t => t.Termin)
                .Include(t => t.Vezbe)
                .FirstOrDefaultAsync(t => t.IdTrening == treningId);

            if (trening == null)
                return NotFound();

            if (!isAdmin && trening.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            var vezba = new Vezba
            {
                TreningId = treningId.Value,
                Redosled = (trening.Vezbe?.Count ?? 0) + 1,
                Serije = 3,
                Ponavljanja = 10
            };

            ViewBag.Trening = trening;
            return View(vezba);
        }

        // POST: Create vežbu
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Create([Bind("TreningId,NazivVezbe,Serije,Ponavljanja,Tezina,Trajanje,Napomene,Redosled")] Vezba vezba)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var trening = await _context.Trening
                .Include(t => t.Termin)
                .FirstOrDefaultAsync(t => t.IdTrening == vezba.TreningId);

            if (trening == null)
                return NotFound();

            if (!isAdmin && trening.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            // Ukloni validation za navigation properties
            ModelState.Remove("Trening");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Vezbe.Add(vezba);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vežba je uspješno dodana.";
                    return RedirectToAction(nameof(Details), new { id = vezba.TreningId });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Greška pri dodavanju vežbe: {ex.Message}");
                    ModelState.AddModelError("", "Greška pri dodavanju vežbe.");
                }
            }

            ViewBag.Trening = trening;
            return View(vezba);
        }

        // GET: Edit vežbu
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var vezba = await _context.Vezbe
                .Include(v => v.Trening)
                .ThenInclude(t => t.Termin)
                .FirstOrDefaultAsync(v => v.IdVezba == id);

            if (vezba == null)
                return NotFound();

            if (!isAdmin && vezba.Trening?.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            return View(vezba);
        }

        // POST: Edit vežbu
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("IdVezba,TreningId,NazivVezbe,Serije,Ponavljanja,Tezina,Trajanje,Napomene,Redosled")] Vezba vezba)
        {
            if (id != vezba.IdVezba)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var originalVezba = await _context.Vezbe
                .Include(v => v.Trening)
                .ThenInclude(t => t.Termin)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.IdVezba == id);

            if (originalVezba == null)
                return NotFound();

            if (!isAdmin && originalVezba.Trening?.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            ModelState.Remove("Trening");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vezba);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vežba je uspješno ažurirana.";
                    return RedirectToAction(nameof(Details), new { id = vezba.TreningId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VezbaExists(vezba.IdVezba))
                        return NotFound();
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Greška pri ažuriranju vežbe: {ex.Message}");
                    ModelState.AddModelError("", "Greška pri ažuriranju vežbe.");
                }
            }

            return View(vezba);
        }

        // GET: Delete vežbu
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var vezba = await _context.Vezbe
                .Include(v => v.Trening)
                .ThenInclude(t => t.Termin)
                .ThenInclude(term => term.Trener)
                .Include(v => v.Trening.Clan)
                .FirstOrDefaultAsync(v => v.IdVezba == id);

            if (vezba == null)
                return NotFound();

            if (!isAdmin && vezba.Trening?.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            return View(vezba);
        }

        // POST: Delete vežbu
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var vezba = await _context.Vezbe
                .Include(v => v.Trening)
                .ThenInclude(t => t.Termin)
                .FirstOrDefaultAsync(v => v.IdVezba == id);

            if (vezba == null)
                return NotFound();

            if (!isAdmin && vezba.Trening?.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            var treningId = vezba.TreningId;

            try
            {
                _context.Vezbe.Remove(vezba);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vežba je uspješno obrisana.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Greška pri brisanju vežbe: {ex.Message}");
                TempData["ErrorMessage"] = "Greška pri brisanju vežbe.";
            }

            return RedirectToAction(nameof(Details), new { id = treningId });
        }

        // GET: Moji treninzi (za članove)
        [Authorize(Roles = "Korisnik")]
        public async Task<IActionResult> MojiTreninzi()
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null) return Forbid();

            var mojiTreninzi = await _context.Trening
                .Include(t => t.Termin)
                .ThenInclude(term => term.Trener)
                .Include(t => t.Vezbe.OrderBy(v => v.Redosled))
                .Where(t => t.ClanId == korisnik.Id)
                .OrderByDescending(t => t.Termin.Datum)
                .ThenByDescending(t => t.Termin.Vrijeme)
                .ToListAsync();

            return View(mojiTreninzi);
        }

        // Helper metode
        private bool TreningExists(int id)
        {
            return _context.Trening.Any(e => e.IdTrening == id);
        }

        private bool VezbaExists(int id)
        {
            return _context.Vezbe.Any(e => e.IdVezba == id);
        }

        // GET: Šabloni vežbi (bonus funkcionalnost)
        [Authorize(Roles = "Trener,Admin")]
        public IActionResult SabloniVezbi(int treningId)
        {
            var sabloni = new List<Vezba>
            {
                new Vezba { NazivVezbe = "Čučanj", Serije = 3, Ponavljanja = 15, TreningId = treningId, Redosled = 1 },
                new Vezba { NazivVezbe = "Sklekovi", Serije = 3, Ponavljanja = 12, TreningId = treningId, Redosled = 2 },
                new Vezba { NazivVezbe = "Mrtvo dizanje", Serije = 3, Ponavljanja = 8, Tezina = 60, TreningId = treningId, Redosled = 3 },
                new Vezba { NazivVezbe = "Bench press", Serije = 3, Ponavljanja = 10, Tezina = 80, TreningId = treningId, Redosled = 4 },
                new Vezba { NazivVezbe = "Trčanje", Trajanje = TimeSpan.FromMinutes(30), TreningId = treningId, Redosled = 5 },
                new Vezba { NazivVezbe = "Plank", Trajanje = TimeSpan.FromMinutes(2), TreningId = treningId, Redosled = 6 }
            };

            ViewBag.TreningId = treningId;
            return View(sabloni);
        }

        // POST: Dodaj šablon vežbe
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trener,Admin")]
        public async Task<IActionResult> DodajSablonVezbe(int treningId, string nazivVezbe, int serije, int ponavljanja, decimal? tezina, TimeSpan? trajanje)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(korisnik, "Admin");

            var trening = await _context.Trening
                .Include(t => t.Termin)
                .Include(t => t.Vezbe)
                .FirstOrDefaultAsync(t => t.IdTrening == treningId);

            if (trening == null)
                return NotFound();

            if (!isAdmin && trening.Termin?.TrenerId != korisnik.Id)
                return Forbid();

            var vezba = new Vezba
            {
                TreningId = treningId,
                NazivVezbe = nazivVezbe,
                Serije = serije,
                Ponavljanja = ponavljanja,
                Tezina = tezina,
                Trajanje = trajanje,
                Redosled = (trening.Vezbe?.Count ?? 0) + 1
            };

            try
            {
                _context.Vezbe.Add(vezba);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Vežba '{nazivVezbe}' je uspješno dodana.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Greška pri dodavanju šablon vežbe: {ex.Message}");
                TempData["ErrorMessage"] = "Greška pri dodavanju vežbe.";
            }

            return RedirectToAction(nameof(SabloniVezbi), new { treningId = treningId });
        }
    }
}