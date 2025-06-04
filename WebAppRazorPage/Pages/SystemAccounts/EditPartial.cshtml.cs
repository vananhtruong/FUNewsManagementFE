using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class EditPartialModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditPartialModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {

            var systemaccount = await _httpClient.GetFromJsonAsync<SystemAccount>($"https://localhost:7126/api/SystemAccount/{id}");
            if (systemaccount == null)
            {
                return NotFound();
            }
            else
            {
                SystemAccount = systemaccount;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                await _httpClient.PutAsJsonAsync($"https://localhost:7126/api/SystemAccount",SystemAccount);
                return RedirectToPage("/SystemAccounts/Index");
            }

            return Page();
        }
    }
}
