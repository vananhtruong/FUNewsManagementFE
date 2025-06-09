using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<SystemAccount> Accounts { get; set; } = Enumerable.Empty<SystemAccount>();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            Accounts = await client.GetFromJsonAsync<List<SystemAccount>>("api/SystemAccount");
        }
    }
}
