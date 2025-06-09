using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();

        public async Task OnGetAsync(string searchString)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            NewsArticles = await client.GetFromJsonAsync<IEnumerable<NewsArticle>>("api/NewsArticle");
            NewsArticles = NewsArticles.OrderByDescending(a => a.CreatedDate).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                NewsArticles = NewsArticles
                    .Where(a => a.NewsTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}
