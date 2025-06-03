using Aplikacija.Data;
using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class PlanIshraneController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanIshraneController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlanIshrane
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PlanIshrane.Include(p => p.Clan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PlanIshrane/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planIshrane = await _context.PlanIshrane
                .Include(p => p.Clan)
                .FirstOrDefaultAsync(m => m.IdPlanishrane == id);
            if (planIshrane == null)
            {
                return NotFound();
            }

            return View(planIshrane);
        }

        // GET: PlanIshrane/Create
        public IActionResult Create()
        {
            ViewData["ClanId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email");
            return View();
        }

        // GET: PlanIshrane/PrikaziCiljeve
        public IActionResult PrikaziCiljeve()
        {
            var ciljevi = Enum.GetValues(typeof(TipCilja)).Cast<TipCilja>().ToList();
            return View(ciljevi);
        }



        // POST: PlanIshrane/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ciljevi,Plan,DatumGenerisanja,Kilaza,Godine")] PlanIshrane planIshrane)
        {
            if (ModelState.IsValid)
            {
                var korisnik = await _context.Korisnik.FirstOrDefaultAsync(k => k.Username == User.Identity.Name);
                if (korisnik == null) return Unauthorized();

                planIshrane.ClanId = korisnik.IdKorisnik;
                _context.Add(planIshrane);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planIshrane);
        }


        // GET: PlanIshrane/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planIshrane = await _context.PlanIshrane.FindAsync(id);
            if (planIshrane == null)
            {
                return NotFound();
            }
            ViewData["ClanId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", planIshrane.ClanId);
            return View(planIshrane);
        }

        // POST: PlanIshrane/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Ciljevi,Plan,DatumGenerisanja,Kilaza,Godine,ClanId")] PlanIshrane planIshrane)
        {
            if (id != planIshrane.IdPlanishrane)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planIshrane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanIshraneExists(planIshrane.IdPlanishrane))
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
            ViewData["ClanId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", planIshrane.ClanId);
            return View(planIshrane);
        }

        // GET: PlanIshrane/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planIshrane = await _context.PlanIshrane
                .Include(p => p.Clan)
                .FirstOrDefaultAsync(m => m.IdPlanishrane == id);
            if (planIshrane == null)
            {
                return NotFound();
            }

            return View(planIshrane);
        }

        // POST: PlanIshrane/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planIshrane = await _context.PlanIshrane.FindAsync(id);
            if (planIshrane != null)
            {
                _context.PlanIshrane.Remove(planIshrane);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanIshraneExists(int id)
        {
            return _context.PlanIshrane.Any(e => e.IdPlanishrane == id);
        }
    }
}
