using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Reports
{
    public class NewDetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NewDetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var article = await client.GetFromJsonAsync<NewsArticle>($"api/NewsArticle/{id}");
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
