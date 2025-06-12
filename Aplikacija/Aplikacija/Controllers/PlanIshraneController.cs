using Aplikacija.Data;
using Aplikacija.Extensions;
using Aplikacija.Models;
using Aplikacija.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Aplikacija.Controllers
{
    [Authorize]
    public class PlanIshraneController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public PlanIshraneController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            IQueryable<PlanIshrane> planoviQuery = _context.PlanIshrane.Include(p => p.Clan);

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains("Admin") && !roles.Contains("Trener"))
            {
                planoviQuery = planoviQuery.Where(p => p.ClanId == currentUser.Id);
            }

            var planovi = await planoviQuery
                .OrderByDescending(p => p.DatumGenerisanja)
                .ToListAsync();

            return View(planovi);
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var roles = await _userManager.GetRolesAsync(currentUser);

            PlanIshrane plan;

            if (roles.Contains("Admin") || roles.Contains("Trener"))
            {
              
                plan = await _context.PlanIshrane
                    .Include(p => p.Clan)
                    .FirstOrDefaultAsync(p => p.IdPlanishrane == id);
            }
            else
            {
                
                plan = await _context.PlanIshrane
                    .Include(p => p.Clan)
                    .FirstOrDefaultAsync(p => p.IdPlanishrane == id && p.ClanId == currentUser.Id);
            }

            if (plan == null) return NotFound();

            var model = new PlanIshraneDetailsViewModel
            {
                Plan = plan,
                BMI = IzracunajBMI(plan.Kilaza, plan.Visina),
                PotrebneKalorije = IzracunajPotrebneKalorije(plan.Kilaza, plan.Visina, plan.Godine, plan.Ciljevi),
                NacinVjezbanja = PreporuceniNacinVjezbanja(plan.Ciljevi)
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            bool postojiPlan = await _context.PlanIshrane.AnyAsync(p => p.ClanId == currentUser.Id);
            if (postojiPlan)
            {
                TempData["ErrorMessage"] = "Već imate aktivan plan ishrane. Ne možete kreirati novi.";
                return RedirectToAction("Index");
            }

            ViewBag.TipCiljaOptions = GetTipCiljaSelectList();
            return View(new PlanIshrane { DatumGenerisanja = DateTime.Now });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlanIshrane model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            if (await _context.PlanIshrane.AnyAsync(p => p.ClanId == currentUser.Id))
            {
                TempData["ErrorMessage"] = "Već imate aktivan plan ishrane. Molimo uredite postojeći.";
                return RedirectToAction("Index");
            }

            ViewBag.TipCiljaOptions = GetTipCiljaSelectList(model.Ciljevi);

            if (!ModelState.IsValid)
                return View(model);

            model.ClanId = currentUser.Id;
            model.DatumGenerisanja = DateTime.Now;
            model.Plan = GenerisiPlanIshrane(model);

            _context.PlanIshrane.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = model.IdPlanishrane });
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var plan = await _context.PlanIshrane.FirstOrDefaultAsync(p => p.IdPlanishrane == id);

            if (plan == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains("Admin") && !roles.Contains("Trener"))
            {
                return Forbid();
            }

            // Ne šalji ClanId za prikaz u View
            ViewBag.TipCiljaOptions = GetTipCiljaSelectList(plan.Ciljevi);
            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlanIshrane model)
        {
            if (id != model.IdPlanishrane) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains("Admin") && !roles.Contains("Trener"))
            {
                return Forbid();
            }

            var originalPlan = await _context.PlanIshrane.AsNoTracking().FirstOrDefaultAsync(p => p.IdPlanishrane == id);
            if (originalPlan == null) return NotFound();
            
            model.ClanId = originalPlan.ClanId;

          
            model.Visina = originalPlan.Visina;

            ViewBag.TipCiljaOptions = GetTipCiljaSelectList(model.Ciljevi);

            if (!ModelState.IsValid) return View(model);

            try
            {
               
                model.Plan = GenerisiPlanIshrane(model);
                model.DatumGenerisanja = DateTime.Now;

                _context.Update(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Plan ishrane je uspješno ažuriran!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanIshraneExists(model.IdPlanishrane)) return NotFound();
                throw;
            }
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var plan = await _context.PlanIshrane
                .Include(p => p.Clan)
                .FirstOrDefaultAsync(p => p.IdPlanishrane == id);

            if (plan == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains("Admin") && !roles.Contains("Trener"))
            {
                return Forbid(); 
            }

            return View(plan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var plan = await _context.PlanIshrane.FirstOrDefaultAsync(p => p.IdPlanishrane == id);
            if (plan == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains("Admin") && !roles.Contains("Trener"))
            {
                return Forbid(); 
            }

            _context.PlanIshrane.Remove(plan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Plan ishrane je uspješno obrisan!";
            return RedirectToAction(nameof(Index));
        }

        private string GenerisiPlanIshrane(PlanIshrane plan)
        {
            return plan.Ciljevi switch
            {
                TipCilja.GubljenjeTezine => @"
Preporučeni plan za gubitak težine:
- Doručak: ovsena kaša sa bobičastim voćem i bademima
- Užina: jogurt sa niskim procentom masti i orašasti plodovi
- Ručak: pečena piletina sa brokolijem i kinoom
- Užina: sveže iseckano povrće (mrkva, krastavac)
- Večera: grilovani losos sa salatom od rukole i paradajza
Preporuka: unositi manje ugljenih hidrata i povećati unos proteina i povrća.
",

                TipCilja.OdrzavanjeTezine => @"
Preporučeni plan za održavanje težine:
- Doručak: integralni tost sa avokadom i kuvanim jajetom
- Užina: sveže voće po izboru
- Ručak: ćuretina sa integralnom rižom i blanširanim povrćem
- Užina: orašasti plodovi i suvo grožđe
- Večera: supa od povrća i pečeni krompir sa grilovanim tikvicama
Preporuka: balansiran unos makronutrijenata sa naglaskom na stabilan unos kalorija.
",

                TipCilja.PovecanjeMuski => @"
Preporučeni plan za povećanje mišićne mase:
- Doručak: omlet sa špinatom i sirom, ovsena kaša sa bananom
- Užina: proteinski šejk i bademi
- Ručak: govedina sa batatom i brokolijem
- Užina: sir sa integralnim krekerima
- Večera: pečena ćuretina sa kinoom i pečenim povrćem
Preporuka: povećati unos proteina i kalorija, redovan unos obroka.
",

                TipCilja.PovecanjeEnergije => @"
Plan za povećanje energije:
- Doručak: smoothie od šumskog voća, banana i bademovog mleka
- Užina: sveže voće i orašasti plodovi
- Ručak: losos sa integralnom rižom i zelenom salatom
- Užina: humus sa štapićima celera i mrkve
- Večera: pečena piletina sa povrćem na pari
Preporuka: unositi hranu bogatu antioksidantima, zdravim mastima i ugljenim hidratima.
",

                TipCilja.OpciZdravstveniCilj => @"
Plan za opšti zdravstveni cilj:
- Doručak: jogurt sa lanenim semenkama i bobičastim voćem
- Užina: sveže voće i orašasti plodovi
- Ručak: salata sa kinoom, avokadom i pečenim povrćem
- Užina: integralni krekeri sa humusom
- Večera: supa od povrća i pečena riba
Preporuka: raznovrsna ishrana sa fokusom na sveže namirnice i balans hranljivih materija.
",

                _ => "Standardni plan ishrane sa uravnoteženim unosom svih nutrijenata."
            };
        }


        private double IzracunajBMI(double kilaza, int visinaCm)
        {
            var visinaM = visinaCm / 100.0;
            return kilaza / (visinaM * visinaM);
        }

        private int IzracunajPotrebneKalorije(double kilaza, int visina, int godine, TipCilja cilj)
        {
            double bmr = 10 * kilaza + 6.25 * visina - 5 * godine + 5;

            return cilj switch
            {
                TipCilja.GubljenjeTezine => (int)(bmr * 1.2 * 0.8),
                TipCilja.OdrzavanjeTezine => (int)(bmr * 1.2),
                TipCilja.PovecanjeMuski => (int)(bmr * 1.2 * 1.2),
                TipCilja.PovecanjeEnergije => (int)(bmr * 1.3),
                TipCilja.OpciZdravstveniCilj => (int)(bmr * 1.1),
                _ => (int)(bmr * 1.2)
            };
        }

        private string PreporuceniNacinVjezbanja(TipCilja cilj)
        {
            return cilj switch
            {
                TipCilja.GubljenjeTezine => "Kardio 4x nedeljno + lagano dizanje tegova",
                TipCilja.OdrzavanjeTezine => "Umeren trening snage 3x nedeljno + lagani kardio",
                TipCilja.PovecanjeMuski => "Intenzivni trening snage 5x nedeljno",
                TipCilja.PovecanjeEnergije => "Kombinacija kardio i joge 3x nedeljno",
                TipCilja.OpciZdravstveniCilj => "Lagani svakodnevni trening i istezanje",
                _ => "Standardni režim vežbanja"
            };
        }

        private bool PlanIshraneExists(int id)
        {
            return _context.PlanIshrane.Any(e => e.IdPlanishrane == id);
        }

        private SelectList GetTipCiljaSelectList(TipCilja? selected = null)
        {
            return new SelectList(
                Enum.GetValues(typeof(TipCilja)).Cast<TipCilja>().Select(c => new
                {
                    Value = c,
                    Text = GetEnumDisplayName(c)
                }),
                "Value", "Text", selected
            );
        }

        private string GetEnumDisplayName(TipCilja cilj)
        {
            var type = typeof(TipCilja);
            var memInfo = type.GetMember(cilj.ToString());
            var attrs = memInfo.FirstOrDefault()?.GetCustomAttributes(typeof(DisplayAttribute), false);
            return (attrs?.FirstOrDefault() as DisplayAttribute)?.Name ?? cilj.ToString();
        }
    }
}
