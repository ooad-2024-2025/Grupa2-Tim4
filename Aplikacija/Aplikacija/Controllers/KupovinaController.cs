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
    public class KupovinaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KupovinaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kupovina
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Kupovina.Include(k => k.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Kupovina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.IdKupovina == id);
            if (kupovina == null)
            {
                return NotFound();
            }

            return View(kupovina);
        }

        // GET: Kupovina/Create
        public IActionResult Create()
        {
            ViewData["IdKorisnik"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email");
            return View();
        }

        // POST: Kupovina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKupovina,DatumKupovine,Artikal,Cijena,Racun,IdKorisnik")] Kupovina kupovina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kupovina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdKorisnik"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", kupovina.IdKorisnik);
            return View(kupovina);
        }

        // GET: Kupovina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina.FindAsync(id);
            if (kupovina == null)
            {
                return NotFound();
            }
            ViewData["IdKorisnik"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", kupovina.IdKorisnik);
            return View(kupovina);
        }

        // POST: Kupovina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKupovina,DatumKupovine,Artikal,Cijena,Racun,IdKorisnik")] Kupovina kupovina)
        {
            if (id != kupovina.IdKupovina)
            {
                return NotFound();
            }

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
            ViewData["IdKorisnik"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", kupovina.IdKorisnik);
            return View(kupovina);
        }

        // GET: Kupovina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.IdKupovina == id);
            if (kupovina == null)
            {
                return NotFound();
            }

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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KupovinaExists(int id)
        {
            return _context.Kupovina.Any(e => e.IdKupovina == id);
        }
    }
}
