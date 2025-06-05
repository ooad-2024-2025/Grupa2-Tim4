using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aplikacija.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            ViewData["ActivePage"] = ManageNavPages.Index;
        }

    }
}
