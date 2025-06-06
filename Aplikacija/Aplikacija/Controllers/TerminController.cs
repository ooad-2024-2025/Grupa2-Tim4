﻿using Aplikacija.Data;
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
    public class TerminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public TerminController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Termin
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Termin.Include(t => t.Trener);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Termin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termin
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(m => m.IdTermin == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // GET: Termin/Create
        public IActionResult Create()
        {
            ViewData["TrenerId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email");
            return View();
        }

        // POST: Termin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Datum,Vrijeme,Vrsta")] Termin termin)
        {
            if (ModelState.IsValid)
            {
                var korisnik = await _userManager.GetUserAsync(User);
                termin.TrenerId = korisnik.Id;



                termin.TrenerId = korisnik.Id;
                _context.Add(termin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(termin);
        }


        // GET: Termin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termin.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }
            ViewData["TrenerId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", termin.TrenerId);
            return View(termin);
        }

        // POST: Termin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Datum,Vrijeme,Vrsta,TrenerId")] Termin termin)
        {
            if (id != termin.IdTermin)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminExists(termin.IdTermin))
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
            ViewData["TrenerId"] = new SelectList(_context.Korisnik, "IdKorisnik", "Email", termin.TrenerId);
            return View(termin);
        }

        // GET: Termin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termin
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(m => m.IdTermin == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // POST: Termin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termin = await _context.Termin.FindAsync(id);
            if (termin != null)
            {
                _context.Termin.Remove(termin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerminExists(int id)
        {
            return _context.Termin.Any(e => e.IdTermin == id);
        }
    }
}
