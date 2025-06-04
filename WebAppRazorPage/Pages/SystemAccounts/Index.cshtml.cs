using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<SystemAccount> Accounts { get; set; } = Enumerable.Empty<SystemAccount>();

        public async Task OnGetAsync()
        {
            Accounts = await _httpClient.GetFromJsonAsync<List<SystemAccount>>("https://localhost:7126/api/SystemAccount");
        }

    }
}
