using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DetailsModel(IHttpClientFactory httpClientFactory)
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
                NewsArticle.Category = await client.GetFromJsonAsync<Category>($"api/Category/{NewsArticle.CategoryId}");
                if (NewsArticle.Tags != null && NewsArticle.Tags.Count > 0)
                {
                    foreach (var tag in NewsArticle.Tags)
                    {
                        tag.NewsArticles = null; // Clear the NewsArticles property to avoid circular references
                    }
                }
            }
            return Page();
        }
    }
}
