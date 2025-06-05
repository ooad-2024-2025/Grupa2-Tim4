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
        public async Task<IActionResult> Create([Bind("Ime,Prezime,Email,CV,Pregledano")] PrijavaZaZaposljavanje prijava)
        {
            Console.WriteLine("Ulaz u POST Create");
            Console.WriteLine("Autentifikovan? " + User.Identity.IsAuthenticated);

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    Console.WriteLine("User null — dodajem test ID privremeno");
                    prijava.KorisnikId = "test-korisnik-id"; // neka test vrijednost
                }
                else
                {
                    prijava.KorisnikId = user.Id;
                }


                prijava.KorisnikId = user.Id;

                if (!User.IsInRole("Admin"))
                {
                    prijava.Pregledano = false;
                }

                _context.Add(prijava);
                await _context.SaveChangesAsync();

                Console.WriteLine("Prijava sačuvana!");

                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine("ModelState nije validan!");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine("GREŠKA: " + error.ErrorMessage);
                }
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
