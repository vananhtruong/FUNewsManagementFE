using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]

    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();

        //public async Task OnGetAsync(string searchString)
        //{
        //    NewsArticles = await _newsArticleService.GetAllNewsArticlesAsync();
        //}
        public async Task OnGetAsync(string searchString)
        {
            NewsArticles = await _httpClient.GetFromJsonAsync<IEnumerable<NewsArticle>>("https://localhost:7126/api/NewsArticle");

            if (!string.IsNullOrEmpty(searchString))
            {
                NewsArticles = NewsArticles
                    .Where(a => a.NewsTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}
