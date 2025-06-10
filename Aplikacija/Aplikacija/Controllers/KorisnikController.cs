using Aplikacija.Data;
using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class KorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public KorisnikController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Korisnik
        [Authorize(Roles = "Admin,Recepcioner")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Korisnik/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var korisnik = await _context.Users.FirstOrDefaultAsync(k => k.Id == id);
            if (korisnik == null)
                return NotFound();

            return View(korisnik);
        }

        // GET: Korisnik/Create
        [Authorize(Roles = "Admin,Recepcioner")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Korisnik/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Recepcioner")]
        public async Task<IActionResult> Create(Korisnik korisnik, string username, string password)
        {
            if (ModelState.IsValid)
            {
                korisnik.UserName = username;
                korisnik.Email = korisnik.Email; 

                var result = await _userManager.CreateAsync(korisnik, password);

                if (result.Succeeded)
                {
                    string roleName = korisnik.Tip switch
                    {
                        TipKorisnika.Administrator => "Admin",
                        TipKorisnika.Clan => "Korisnik",
                        TipKorisnika.Recepcioner => "Recepcioner",
                        TipKorisnika.Trener => "Trener",
                        _ => "Korisnik"
                    };

                    await _userManager.AddToRoleAsync(korisnik, roleName);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(korisnik);
        }

        // GET: Korisnik/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var korisnik = await _context.Users.FindAsync(id);
            if (korisnik == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user.Id != id && !User.IsInRole("Admin"))
                return Forbid();

            return View(korisnik);
        }

        // POST: Korisnik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Ime,Prezime,Tip")] Korisnik korisnik)
        {
            // DEBUG: ispis podataka koje forma šalje
            System.Diagnostics.Debug.WriteLine($"[Edit.POST] id iz URL-a: {id}");
            System.Diagnostics.Debug.WriteLine($"[Edit.POST] korisnik.Id: {korisnik.Id}");
            System.Diagnostics.Debug.WriteLine($"[Edit.POST] korisnik.Ime: {korisnik.Ime}");
            System.Diagnostics.Debug.WriteLine($"[Edit.POST] korisnik.Prezime: {korisnik.Prezime}");
            System.Diagnostics.Debug.WriteLine($"[Edit.POST] korisnik.Tip: {korisnik.Tip}");

            if (id != korisnik.Id)
            {
                System.Diagnostics.Debug.WriteLine("[Edit.POST] ID mismatch");
                return NotFound();
            }

            // Provjera validacije modela
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("[Edit.POST] ModelState nije validan:");
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($" - {kvp.Key}: {error.ErrorMessage}");
                    }
                }
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var existing = await _userManager.FindByIdAsync(id);

            if (existing == null)
            {
                System.Diagnostics.Debug.WriteLine("[Edit.POST] Korisnik ne postoji");
                return NotFound();
            }

            if (existing.Id != currentUser.Id && !User.IsInRole("Admin"))
            {
                System.Diagnostics.Debug.WriteLine("[Edit.POST] Nemaš prava da edituješ ovog korisnika.");
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existing.Ime = korisnik.Ime;
                    existing.Prezime = korisnik.Prezime;

                    if (User.IsInRole("Admin") && existing.Tip != korisnik.Tip)
                    {
                        string oldRole = existing.Tip.ToString();
                        string newRole = korisnik.Tip switch
                        {
                            TipKorisnika.Administrator => "Admin",
                            TipKorisnika.Clan => "Korisnik",
                            TipKorisnika.Recepcioner => "Recepcioner",
                            TipKorisnika.Trener => "Trener",
                            _ => "Korisnik"
                        };


                        System.Diagnostics.Debug.WriteLine($"[Edit.POST] Mijenjam rolu: {oldRole} => {newRole}");

                        await _userManager.RemoveFromRoleAsync(existing, oldRole);
                        await _userManager.AddToRoleAsync(existing, newRole);

                        existing.Tip = korisnik.Tip;
                    }

                    var updateResult = await _userManager.UpdateAsync(existing);
                    if (!updateResult.Succeeded)
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"[Edit.POST] Update error: {error.Description}");
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("[Edit.POST] Ažuriranje korisnika je završeno.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Edit.POST] Exception: {ex.Message}");
                    throw;
                }
            }

            return View(korisnik);
        }



        // GET: Korisnik/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var korisnik = await _context.Users.FirstOrDefaultAsync(k => k.Id == id);
            if (korisnik == null)
                return NotFound();

            return View(korisnik);
        }

        // POST: Korisnik/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var korisnik = await _context.Users.FirstOrDefaultAsync(k => k.Id == id);
            if (korisnik == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(korisnik);
            if (roles.Any())
                await _userManager.RemoveFromRolesAsync(korisnik, roles);

            await _userManager.DeleteAsync(korisnik);
            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }







        public async Task<IActionResult> MojProfil()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", new { id = user.Id });
        }



    }
}
