using Aplikacija.Data;
using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class PrijavaZaZaposljavanjeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public PrijavaZaZaposljavanjeController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PrijavaZaZaposljavanje
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PrijavaZaZaposljavanje.Include(p => p.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PrijavaZaZaposljavanje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaZaZaposljavanje = await _context.PrijavaZaZaposljavanje
                .Include(p => p.Korisnik)
                .FirstOrDefaultAsync(m => m.IdPrijava == id);
            if (prijavaZaZaposljavanje == null)
            {
                return NotFound();
            }

            return View(prijavaZaZaposljavanje);
        }

        // GET: PrijavaZaZaposljavanje/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PrijavaZaZaposljavanje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrijavaZaZaposljavanje prijava)
        {
            System.Diagnostics.Debug.WriteLine("=== [PRIJAVA.POST] Start ===");
            System.Diagnostics.Debug.WriteLine($"Ime: {prijava.Ime}");
            System.Diagnostics.Debug.WriteLine($"Prezime: {prijava.Prezime}");
            System.Diagnostics.Debug.WriteLine($"Email: {prijava.Email}");
            System.Diagnostics.Debug.WriteLine($"CV: {prijava.CV}");
            System.Diagnostics.Debug.WriteLine($"Pregledano (input): {prijava.Pregledano}");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine("[PRIJAVA.POST] Korisnik nije prijavljen.");
                return Unauthorized();
            }

            prijava.KorisnikId = user.Id;

            // ⬇️ OVDJE uklanjaš grešku za KorisnikId (jer je sad ručno postavljen)
            ModelState.Remove(nameof(prijava.KorisnikId));

            if (!User.IsInRole("Admin"))
            {
                prijava.Pregledano = false;
            }

            if (ModelState.IsValid)
            {
                _context.Add(prijava);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("[PRIJAVA.POST] Prijava sačuvana.");
                return RedirectToAction("Index", "Home");
            }

            System.Diagnostics.Debug.WriteLine("[PRIJAVA.POST] ModelState nije validan:");
            foreach (var kvp in ModelState)
            {
                foreach (var error in kvp.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($" - {kvp.Key}: {error.ErrorMessage}");
                }
            }

            return View(prijava);
        }



        // GET: PrijavaZaZaposljavanje/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaZaZaposljavanje = await _context.PrijavaZaZaposljavanje.FindAsync(id);
            if (prijavaZaZaposljavanje == null)
            {
                return NotFound();
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", prijavaZaZaposljavanje.KorisnikId);
            return View(prijavaZaZaposljavanje);
        }

        // POST: PrijavaZaZaposljavanje/Edit/5

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Ime,Prezime,Email,CV,Pregledano,KorisnikId")] PrijavaZaZaposljavanje prijavaZaZaposljavanje)
        {
            if (id != prijavaZaZaposljavanje.IdPrijava)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prijavaZaZaposljavanje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrijavaZaZaposljavanjeExists(prijavaZaZaposljavanje.IdPrijava))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", prijavaZaZaposljavanje.KorisnikId);
            return View(prijavaZaZaposljavanje);
        }

        // GET: PrijavaZaZaposljavanje/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaZaZaposljavanje = await _context.PrijavaZaZaposljavanje
                .Include(p => p.Korisnik)
                .FirstOrDefaultAsync(m => m.IdPrijava == id);
            if (prijavaZaZaposljavanje == null)
            {
                return NotFound();
            }

            return View(prijavaZaZaposljavanje);
        }

        // POST: PrijavaZaZaposljavanje/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prijavaZaZaposljavanje = await _context.PrijavaZaZaposljavanje.FindAsync(id);
            if (prijavaZaZaposljavanje != null)
            {
                _context.PrijavaZaZaposljavanje.Remove(prijavaZaZaposljavanje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrijavaZaZaposljavanjeExists(int id)
        {
            return _context.PrijavaZaZaposljavanje.Any(e => e.IdPrijava == id);
        }
    }
}
