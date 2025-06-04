using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class CreatePartialModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = new();

        public CreatePartialModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingAccounts = await _httpClient.PostAsJsonAsync("https://localhost:7126/api/SystemAccount/Any", SystemAccount);

            if (existingAccounts.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "❌ Account ID đã tồn tại. Vui lòng nhập ID khác.";
                return RedirectToPage("./Index");
            }

            await _httpClient.PostAsJsonAsync("https://localhost:7126/api/SystemAccount", SystemAccount);
            return RedirectToPage("./Index");
        }
    }
}
