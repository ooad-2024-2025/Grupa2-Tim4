using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aplikacija.Data;
using Aplikacija.Models;

namespace Aplikacija.Controllers
{
    public class PrijavaZaZaposljavanjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrijavaZaZaposljavanjeController(ApplicationDbContext context)
        {
            _context = context;
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email");
            return View();
        }

        // POST: PrijavaZaZaposljavanje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Prezime,Email,CV,Pregledano")] PrijavaZaZaposljavanje prijava)
        {
            if (ModelState.IsValid)
            {
                var korisnik = await _context.Korisnik.FirstOrDefaultAsync(k => k.Username == User.Identity.Name);
                if (korisnik == null) return Unauthorized();

                prijava.KorisnikId = korisnik.IdKorisnik;
                _context.Add(prijava);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prijava);
        }


        // GET: PrijavaZaZaposljavanje/Edit/5
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
