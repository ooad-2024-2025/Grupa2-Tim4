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
            return View(await _context.PlanIshrane.ToListAsync());
        }

        // GET: PlanIshrane/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planIshrane = await _context.PlanIshrane
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planIshrane == null)
            {
                return NotFound();
            }

            return View(planIshrane);
        }

        // GET: PlanIshrane/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlanIshrane/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] PlanIshrane planIshrane)
        {
            if (ModelState.IsValid)
            {
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
            return View(planIshrane);
        }

        // POST: PlanIshrane/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] PlanIshrane planIshrane)
        {
            if (id != planIshrane.Id)
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
                    if (!PlanIshraneExists(planIshrane.Id))
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
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return _context.PlanIshrane.Any(e => e.Id == id);
        }
    }
}
