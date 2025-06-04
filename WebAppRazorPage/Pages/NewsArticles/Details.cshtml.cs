using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]

    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(HttpClient httpClient)
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
               NewsArticle.Category = await _httpClient.GetFromJsonAsync<Category>($"https://localhost:7126/api/Category/{NewsArticle.CategoryId}");
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
