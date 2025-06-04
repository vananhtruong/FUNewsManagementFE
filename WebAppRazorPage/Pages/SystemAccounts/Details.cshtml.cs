using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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
    }
}
