using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Aplikacija.Models;

namespace Aplikacija.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Korisnik> _signInManager;

        public AccountController(SignInManager<Korisnik> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}