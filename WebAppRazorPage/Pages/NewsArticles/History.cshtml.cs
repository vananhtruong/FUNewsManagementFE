using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazorPage.Pages.NewsArticles
{
    public class HistoryModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HistoryModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();

        public async Task OnGetAsync()
        {
            var accountId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var client = _httpClientFactory.CreateClient("MyApi");
            if (!string.IsNullOrEmpty(accountId))
            {
                NewsArticles = await client.GetFromJsonAsync<IEnumerable<NewsArticle>>($"api/NewsArticle/staff/{accountId}");
            }
            else
            {
                NewsArticles = Enumerable.Empty<NewsArticle>();
            }
        }
    }
}
