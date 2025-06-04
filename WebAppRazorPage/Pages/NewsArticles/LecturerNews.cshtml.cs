using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    public class LecturerNewsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LecturerNewsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }


        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();

      
        public async Task OnGetAsync(string id)
        {
            NewsArticles = await _httpClient.GetFromJsonAsync<IEnumerable<NewsArticle>>("https://localhost:7126/api/NewsArticle/active");

        }

    }
}
