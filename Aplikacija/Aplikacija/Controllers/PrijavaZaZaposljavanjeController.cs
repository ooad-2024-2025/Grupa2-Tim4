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
            return View(await _context.PrijavaZaZaposljavanje.ToListAsync());
        }

        // GET: PrijavaZaZaposljavanje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaZaZaposljavanje = await _context.PrijavaZaZaposljavanje
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id")] PrijavaZaZaposljavanje prijavaZaZaposljavanje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prijavaZaZaposljavanje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prijavaZaZaposljavanje);
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
            return View(prijavaZaZaposljavanje);
        }

        // POST: PrijavaZaZaposljavanje/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] PrijavaZaZaposljavanje prijavaZaZaposljavanje)
        {
            if (id != prijavaZaZaposljavanje.Id)
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
                    if (!PrijavaZaZaposljavanjeExists(prijavaZaZaposljavanje.Id))
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
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return _context.PrijavaZaZaposljavanje.Any(e => e.Id == id);
        }
    }
}
