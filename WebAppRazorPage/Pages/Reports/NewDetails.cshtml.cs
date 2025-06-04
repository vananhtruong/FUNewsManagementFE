using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Reports
{
    public class NewDetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public NewDetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var article = await _httpClient.GetFromJsonAsync<NewsArticle>($"https://localhost:7126/api/NewsArticle/{id}");
            if (article == null)
            {
                return NotFound();
            }
            else
            {
                NewsArticle = article;
            }
            return Page();
        }
    }
}
