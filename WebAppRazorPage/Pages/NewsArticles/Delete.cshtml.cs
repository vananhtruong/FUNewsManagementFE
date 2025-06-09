using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            await client.DeleteAsync($"api/NewsArticle/{id}");
            return RedirectToPage("/NewsArticles/Index");
        }
    }
}
