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
    public class TreningController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public TreningController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Trening
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Trening.Include(t => t.Clan).Include(t => t.Trener);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Trening/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.Trening
                .Include(t => t.Clan)
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(m => m.IdTrening == id);
            if (trening == null)
            {
                return NotFound();
            }

            return View(trening);
        }

        // GET: Trening/Create
        public IActionResult Create()
        {
            ViewData["ClanId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email");
            ViewData["TrenerId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email");
            return View();
        }

        // POST: Trening/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Datum,Vrijeme,Tip")] Trening trening)
        {
            if (ModelState.IsValid)
            {
                var korisnik = await _userManager.GetUserAsync(User);
                if (korisnik.Tip == TipKorisnika.Clan)
                    trening.ClanId = korisnik.Id;


                if (korisnik.Tip == TipKorisnika.Clan)
                    trening.ClanId = korisnik.Id;
                else if (korisnik.Tip == TipKorisnika.Trener)
                    trening.TrenerId = korisnik.Id;

                _context.Add(trening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(trening);
        }


        // GET: Trening/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.Trening.FindAsync(id);
            if (trening == null)
            {
                return NotFound();
            }
            ViewData["ClanId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", trening.ClanId);
            ViewData["TrenerId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", trening.TrenerId);
            return View(trening);
        }

        // POST: Trening/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Datum,Vrijeme,Tip,ClanId,TrenerId")] Trening trening)
        {
            if (id != trening.IdTrening)
            {
                return NotFound();
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
            ViewData["ClanId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", trening.ClanId);
            ViewData["TrenerId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", trening.TrenerId);
            return View(trening);
        }

        // GET: Trening/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.Trening
                .Include(t => t.Clan)
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(m => m.IdTrening == id);
            if (trening == null)
            {
                return NotFound();
            }

            return View(trening);
        }

        // POST: Trening/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trening = await _context.Trening.FindAsync(id);
            if (trening != null)
            {
                _context.Trening.Remove(trening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreningExists(int id)
        {
            return _context.Trening.Any(e => e.IdTrening == id);
        }
    }
}
