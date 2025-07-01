using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    public class EditPartialModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditPartialModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var systemaccount = await client.GetFromJsonAsync<SystemAccount>($"api/SystemAccount/{id}");
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
            var client = _httpClientFactory.CreateClient("MyApi");
            if (ModelState.IsValid)
            {
                await client.PutAsJsonAsync("api/SystemAccount", SystemAccount);
                return RedirectToPage("/SystemAccounts/Index");
            }

            return Page();
        }
    }
}
