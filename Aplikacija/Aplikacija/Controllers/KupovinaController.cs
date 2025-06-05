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
    public class KupovinaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public KupovinaController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Kupovina
        public async Task<IActionResult> Index()
        {
            var kupovine = _context.Kupovina.Include(k => k.Korisnik);
            return View(await kupovine.ToListAsync());
        }

        // GET: Kupovina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var kupovina = await _context.Kupovina
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.IdKupovina == id);

            if (kupovina == null)
                return NotFound();

            return View(kupovina);
        }

        // GET: Kupovina/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kupovina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DatumKupovine,Artikal,Cijena,Racun")] Kupovina kupovina)
        {
            if (ModelState.IsValid)
            {
                var korisnik = await _userManager.GetUserAsync(User);
                if (korisnik == null) return Unauthorized();

                kupovina.KorisnikId = korisnik.Id;

                _context.Add(kupovina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kupovina);
        }

        // GET: Kupovina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var kupovina = await _context.Kupovina
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(k => k.IdKupovina == id);

            if (kupovina == null)
                return NotFound();

            ViewBag.Korisnici = new SelectList(_context.Users.ToList(), "Id", "Email", kupovina.Korisnik?.Id);
            return View(kupovina);
        }


        // POST: Kupovina/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKupovina,DatumKupovine,Artikal,Cijena,Racun,KorisnikId")] Kupovina kupovina)
        {
            if (id != kupovina.IdKupovina)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kupovina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KupovinaExists(kupovina.IdKupovina))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kupovina);
        }

        // GET: Kupovina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var kupovina = await _context.Kupovina
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.IdKupovina == id);
            if (kupovina == null)
                return NotFound();

            return View(kupovina);
        }

        // POST: Kupovina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kupovina = await _context.Kupovina.FindAsync(id);
            if (kupovina != null)
            {
                _context.Kupovina.Remove(kupovina);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KupovinaExists(int id)
        {
            return _context.Kupovina.Any(e => e.IdKupovina == id);
        }
    }
}
