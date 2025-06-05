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
    public class KorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public KorisnikController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Korisnik
        public async Task<IActionResult> Index()
        {
            return View(await _context.Korisnik.ToListAsync());
        }

        // GET: Korisnik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(m => m.IdKorisnik == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // GET: Korisnik/Create
        [Authorize(Roles = "Admin,Recepcioner")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Korisnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Korisnik/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Recepcioner")]
        public async Task<IActionResult> Create([Bind("Ime,Prezime,Username,Password,Email,Tip")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                // 1. Kreiraj Identity korisnika
                var identityUser = new IdentityUser
                {
                    UserName = korisnik.Username,
                    Email = korisnik.Email
                };

                var result = await _userManager.CreateAsync(identityUser, korisnik.Password);

                if (result.Succeeded)
                {
                    // 2. Mapiraj enum TipKorisnika u tačan naziv role iz AspNetRoles
                    string roleName = korisnik.Tip switch
                    {
                        TipKorisnika.Administrator => "Admin",
                        TipKorisnika.Clan => "Korisnik",
                        TipKorisnika.Recepcioner => "Recepcioner",
                        TipKorisnika.Trener => "Trener",
                        _ => "Korisnik"
                    };

                    // 3. Dodaj ulogu korisniku
                    var roleResult = await _userManager.AddToRoleAsync(identityUser, roleName);

                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(korisnik);
                    }

                    // 4. Spasi u domensku tabelu
                    korisnik.IdentityUserId = identityUser.Id;
                    _context.Korisnik.Add(korisnik);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Ako kreiranje korisnika nije uspjelo
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(korisnik);
        }



        // GET: Korisnik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var korisnik = await _context.Korisnik.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);


            return View(korisnik);
        }

        // POST: Korisnik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKorisnik,Ime,Prezime,Username,Password,Email,Tip")] Korisnik korisnik)
        {
            var user = await _userManager.GetUserAsync(User);
            var existing = await _context.Korisnik.AsNoTracking().FirstOrDefaultAsync(k => k.IdKorisnik == id);


            if (id != korisnik.IdKorisnik)
                return NotFound();

            if (existing.IdentityUserId != user.Id && !User.IsInRole("Admin"))
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    korisnik.IdentityUserId = existing.IdentityUserId;
                    _context.Update(korisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                    throw;
                }
                return RedirectToAction("Index", "Home");
            }

            return View(korisnik);
        }

        [Authorize(Roles = "Admin")]
        // GET: Korisnik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(m => m.IdKorisnik == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _context.Korisnik
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.IdKorisnik == id);

            if (korisnik == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(korisnik.IdentityUserId);

            if (user != null)
            {
                
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, roles);
                }

                
                await _userManager.DeleteAsync(user);
            }

            
            _context.Attach(korisnik);
            _context.Korisnik.Remove(korisnik);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        private bool KorisnikExists(int id)
        {
            return _context.Korisnik.Any(e => e.IdKorisnik == id);
        }
    }
}