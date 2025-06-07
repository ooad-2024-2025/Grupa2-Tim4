using Aplikacija.Data;
using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija.Controllers
{

    public class Proizvod
    {
        public string Naziv { get; set; }
        public float Cijena { get; set; }
        public string Slika { get; set; }
    }


    public class KorpaItem
    {
        public float cijena { get; set; }
        public int kolicina { get; set; }
    }


    public class StavkaNarudzbe
    {
        public string Naziv { get; set; }
        public int Kolicina { get; set; }
    }

    public class NarudzbaViewModel
    {
        public string Racun { get; set; }
        public string Korisnik { get; set; }
        public DateOnly Datum { get; set; }
        public List<StavkaNarudzbe> Stavke { get; set; }
        public float Ukupno { get; set; }
        public int IdKupovina { get; set; }
    }


    [Authorize]
    public class KupovinaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public KupovinaController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: Kupovina
        [Authorize(Roles = "Admin,Recepcioner")]
        public async Task<IActionResult> Index()
        {
            var sveKupovine = await _context.Kupovina
                .Include(k => k.Korisnik)
                .ToListAsync();

            var narudzbe = sveKupovine
                .GroupBy(k => new { k.Racun, k.Korisnik.Email, k.DatumKupovine })
                .Select(g => new NarudzbaViewModel
                {
                    Racun = g.Key.Racun,
                    Korisnik = g.Key.Email,
                    Datum = g.Key.DatumKupovine,
                    Stavke = g.GroupBy(x => x.Artikal)
                              .Select(art => new StavkaNarudzbe
                              {
                                  Naziv = art.Key,
                                  Kolicina = art.Count()
                              }).ToList(),
                    Ukupno = g.Sum(x => x.Cijena)
                }).ToList();

            ViewBag.Narudzbe = narudzbe;

            return View();
        }



        private float DohvatiCijenuArtikla(string naziv)
        {
            return DohvatiProizvode().FirstOrDefault(p => p.Naziv == naziv)?.Cijena ?? 1f;
        }



        // GET: Kupovina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var kupovina = await _context.Kupovina
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.IdKupovina == id);

            if (kupovina == null)
                return NotFound();

            return View(kupovina);
        }

        // GET: Kupovina/Create
        public IActionResult Create()
        {
            return View(DohvatiProizvode());
        }


        // POST: Kupovina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DatumKupovine,Artikal,Cijena,Racun")] Kupovina kupovina)
        {
            System.Diagnostics.Debug.WriteLine("[Create.POST] Pozvana metoda Create");
            System.Diagnostics.Debug.WriteLine($"[Create.POST] Podaci iz forme: Artikal={kupovina.Artikal}, Cijena={kupovina.Cijena}, Datum={kupovina.DatumKupovine}, Racun={kupovina.Racun}");

            if (ModelState.IsValid)
            {
                var korisnik = await _userManager.GetUserAsync(User);
                if (korisnik == null)
                {
                    System.Diagnostics.Debug.WriteLine("[Create.POST] Korisnik nije prijavljen.");
                    return Unauthorized();
                }

                kupovina.KorisnikId = korisnik.Id;

                _context.Add(kupovina);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("[Create.POST] Kupovina je spašena u bazu.");
                return RedirectToAction(nameof(Index));
            }

            System.Diagnostics.Debug.WriteLine("[Create.POST] ModelState nije validan.");
            foreach (var kvp in ModelState)
            {
                foreach (var error in kvp.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($" - {kvp.Key}: {error.ErrorMessage}");
                }
            }

            return View(kupovina);
        }


        // GET: Kupovina/Edit/5
        [Authorize(Roles = "Admin,Recepcioner")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var kupovina = await _context.Kupovina
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(k => k.IdKupovina == id);

            if (kupovina == null)
                return NotFound();

            ViewBag.Korisnici = new SelectList(_context.Users.ToList(), "Id", "Email", kupovina.Korisnik?.Id);
            return View(kupovina);
        }


        // POST: Kupovina/Edit/5
        [Authorize(Roles = "Admin,Recepcioner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKupovina,DatumKupovine,Artikal,Cijena,Racun,KorisnikId")] Kupovina kupovina)
        {
            System.Diagnostics.Debug.WriteLine("[Edit.POST] Pozvana metoda Edit");
            System.Diagnostics.Debug.WriteLine($"[Edit.POST] id iz URL-a: {id}");
            System.Diagnostics.Debug.WriteLine($"[Edit.POST] kupovina.IdKupovina: {kupovina.IdKupovina}, Artikal: {kupovina.Artikal}, Cijena: {kupovina.Cijena}");

            if (id != kupovina.IdKupovina)
            {
                System.Diagnostics.Debug.WriteLine("[Edit.POST] ID mismatch!");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kupovina);
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("[Edit.POST] Uspješno ažurirano.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KupovinaExists(kupovina.IdKupovina))
                    {
                        System.Diagnostics.Debug.WriteLine("[Edit.POST] Kupovina ne postoji u bazi.");
                        return NotFound();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("[Edit.POST] DbUpdateConcurrencyException bačen!");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            System.Diagnostics.Debug.WriteLine("[Edit.POST] ModelState nije validan.");
            foreach (var kvp in ModelState)
            {
                foreach (var error in kvp.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($" - {kvp.Key}: {error.ErrorMessage}");
                }
            }

            return View(kupovina);
        }


        // GET: Kupovina/Delete/5
        [Authorize(Roles = "Admin,Recepcioner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var kupovina = await _context.Kupovina
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.IdKupovina == id);
            if (kupovina == null)
                return NotFound();

            return View(kupovina);
        }

        // POST: Kupovina/Delete/5
        [Authorize(Roles = "Admin,Recepcioner")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kupovina = await _context.Kupovina.FindAsync(id);
            if (kupovina != null)
            {
                _context.Kupovina.Remove(kupovina);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Kupovina/DeleteByRacun?racun=RN20250606123456
        [Authorize(Roles = "Admin,Recepcioner")]
        public async Task<IActionResult> DeleteByRacun(string racun)
        {
            if (string.IsNullOrEmpty(racun))
                return NotFound();

            var kupovine = await _context.Kupovina
                .Include(k => k.Korisnik)
                .Where(k => k.Racun == racun)
                .ToListAsync();

            if (!kupovine.Any())
                return NotFound();

            return View(kupovine);
        }

        // POST: Kupovina/DeleteByRacun
        [Authorize(Roles = "Admin,Recepcioner")]
        [HttpPost, ActionName("DeleteByRacun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteByRacunConfirmed(string racun)
        {
            if (string.IsNullOrEmpty(racun))
                return NotFound();

            var kupovine = await _context.Kupovina
                .Where(k => k.Racun == racun)
                .ToListAsync();

            if (!kupovine.Any())
                return NotFound();

            _context.Kupovina.RemoveRange(kupovine);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        private bool KupovinaExists(int id)
        {
            return _context.Kupovina.Any(e => e.IdKupovina == id);
        }




        public async Task<IActionResult> Korpa()
        {
            var user = await _userManager.GetUserAsync(User);
            var kupovine = await _context.Kupovina
                .Include(k => k.Korisnik)
                .Where(k => k.KorisnikId == user.Id)
                .ToListAsync();

            if (kupovine == null || kupovine.Count == 0)
            {
                ViewBag.PraznaKorpa = true;
                return View("KorpaPrazna");
            }

            return RedirectToAction("DetailsByRacun", new { racun = kupovine.First().Racun });
        }





        private List<Proizvod> DohvatiProizvode()
        {
            return new List<Proizvod>
        {
            new Proizvod { Naziv = "Tegovi 10kg", Cijena = 49.99f, Slika = "/images/tegovi.jpg" },
            new Proizvod { Naziv = "Matična podloga", Cijena = 29.99f, Slika = "/images/podloga.jpg" },
            new Proizvod { Naziv = "Traka za trčanje", Cijena = 699.99f, Slika = "/images/traka.jpg" },
            new Proizvod { Naziv = "Boca za vodu", Cijena = 12.99f, Slika = "/images/boca.jpg" },
            new Proizvod { Naziv = "Fitnes rukavice", Cijena = 19.99f, Slika = "/images/rukavice.jpg" }
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrdiNarudzbu([FromForm] string json)
        {
            System.Diagnostics.Debug.WriteLine("[PotvrdiNarudzbu.POST] Pozvana metoda");

            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null)
            {
                System.Diagnostics.Debug.WriteLine("[PotvrdiNarudzbu.POST] Korisnik nije prijavljen.");
                return BadRequest("Korisnik nije prijavljen.");
            }

            if (string.IsNullOrEmpty(json))
            {
                System.Diagnostics.Debug.WriteLine("[PotvrdiNarudzbu.POST] JSON string je prazan.");
                return BadRequest("Korpa je prazna.");
            }

            Dictionary<string, KorpaItem> artikli;
            try
            {
                artikli = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, KorpaItem>>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PotvrdiNarudzbu.POST] Greška u deserializaciji: {ex.Message}");
                return BadRequest("Neispravan format podataka.");
            }

            if (artikli == null || !artikli.Any())
            {
                System.Diagnostics.Debug.WriteLine("[PotvrdiNarudzbu.POST] Deserializirano, ali artikli su prazni.");
                return BadRequest("Nema artikala.");
            }

            System.Diagnostics.Debug.WriteLine($"[PotvrdiNarudzbu.POST] Ukupno artikala: {artikli.Count}");

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var racunId = "RN" + timestamp;

            foreach (var item in artikli)
            {
                System.Diagnostics.Debug.WriteLine($"[PotvrdiNarudzbu.POST] Artikal: {item.Key}, Cijena: {item.Value.cijena}, Količina: {item.Value.kolicina}");

                var novaKupovina = new Kupovina
                {
                    DatumKupovine = DateOnly.FromDateTime(DateTime.Now),
                    Artikal = item.Key,
                    Cijena = item.Value.cijena * item.Value.kolicina,
                    Racun = racunId,
                    KorisnikId = korisnik.Id
                };
                _context.Kupovina.Add(novaKupovina);
            }

            await _context.SaveChangesAsync();
            System.Diagnostics.Debug.WriteLine("[PotvrdiNarudzbu.POST] Sve kupovine su spremljene.");

            // ⏩ Preusmjerenje na prikaz detalja narudžbe
            return RedirectToAction("DetailsByRacun", new { racun = racunId });
        }


        public async Task<IActionResult> DetailsByRacun(string racun)
        {
            var narudzba = await _context.Kupovina
                .Include(k => k.Korisnik)
                .Where(k => k.Racun == racun)
                .ToListAsync();

            if (!narudzba.Any()) return NotFound();

            return View(narudzba);
        }


    }
}
