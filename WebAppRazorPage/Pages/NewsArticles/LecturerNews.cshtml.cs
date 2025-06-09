using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    public class LecturerNewsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LecturerNewsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();

        public async Task OnGetAsync(string id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            NewsArticles = await client.GetFromJsonAsync<IEnumerable<NewsArticle>>("api/NewsArticle/active");
        }
    }
}
