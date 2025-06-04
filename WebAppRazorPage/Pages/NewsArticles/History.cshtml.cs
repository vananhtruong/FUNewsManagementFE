using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazorPage.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]

    public class HistoryModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public HistoryModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }


        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();


        public async Task OnGetAsync()
        {
            var accountId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(accountId))
            {
                NewsArticles = await _httpClient.GetFromJsonAsync<IEnumerable<NewsArticle>>($"https://localhost:7126/api/NewsArticle/staff/{accountId}");
            }
            else
            {
                NewsArticles = Enumerable.Empty<NewsArticle>();
            }
        }

    }

}
