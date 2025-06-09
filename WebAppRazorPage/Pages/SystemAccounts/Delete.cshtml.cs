using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnPostAsync(short id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var response = await client.DeleteAsync($"api/SystemAccount/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/SystemAccounts/Index");
            }
            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(error);
        }
    }
}
