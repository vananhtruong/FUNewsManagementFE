using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazorPage.Pages.SystemAccounts
{
    public class GoogleLoginModel : PageModel
    {
        public IActionResult OnGet()
        {
            var redirectUrl = Url.Page("/SystemAccounts/GoogleResponse");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }
    }
}
