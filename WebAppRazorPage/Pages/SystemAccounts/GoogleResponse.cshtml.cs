using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebAppRazorPage.Model;

namespace WebAppRazorPage.Pages.SystemAccounts
{
    public class GoogleResponseModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleResponseModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");
            if (!authenticateResult.Succeeded)
                return RedirectToPage("/SystemAccounts/Login");

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;

            var logingooglemodel = new
            {
                AccountEmail = email,
                AccountName = name,
                AccountRole = 2
            };

            var client = _httpClientFactory.CreateClient("MyApi");
            var response = await client.PostAsJsonAsync("api/SystemAccount/loginwithgoogle", logingooglemodel);
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<SystemAccount>();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                    new Claim(ClaimTypes.Name, user.AccountName ?? ""),
                    new Claim(ClaimTypes.Email, user.AccountEmail ?? ""),
                    new Claim(ClaimTypes.Role, "Lecturer")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal);

                return RedirectToPage("/NewsArticles/LecturerNews");
            }
            // Nếu lỗi
            TempData["SuccessMessage"] = "Không thể đăng nhập bằng Google, thử lại.";
            return RedirectToPage("/SystemAccounts/Login");
        }
    }
}
