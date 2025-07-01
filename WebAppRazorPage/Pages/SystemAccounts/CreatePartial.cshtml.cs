using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    public class CreatePartialModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = new();

        public CreatePartialModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");

            var existingAccounts = await client.PostAsJsonAsync("api/SystemAccount/Any", SystemAccount);

            if (existingAccounts.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "❌ Account ID đã tồn tại. Vui lòng nhập ID khác.";
                return RedirectToPage("./Index");
            }

            await client.PostAsJsonAsync("api/SystemAccount", SystemAccount);
            return RedirectToPage("./Index");
        }
    }
}
