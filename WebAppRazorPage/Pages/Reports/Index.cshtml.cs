using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public List<NewsArticle> NewsArticles { get; set; }
        public int CountArticles { get; set; }

        public Dictionary<string, int> CategoryStats { get; set; }
        public Dictionary<string, int> StaffStats { get; set; }

        public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            StartDate ??= startDate;
            EndDate ??= endDate;
            await LoadDataAsync();
            return Page();
        }

        private async Task LoadDataAsync()
        {
            var articles = await _httpClient.GetFromJsonAsync<List<NewsArticle>>("https://localhost:7126/api/NewsArticle")
               ?? new List<NewsArticle>();

            if (StartDate.HasValue)
                articles = articles.Where(a => a.CreatedDate >= StartDate.Value).ToList();
            if (EndDate.HasValue)
                articles = articles.Where(a => a.CreatedDate <= EndDate.Value).ToList();

            NewsArticles = articles.OrderByDescending(a => a.CreatedDate).ToList();
            CountArticles = NewsArticles.Count;

            var categories = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7126/api/Category")
                ?? new List<Category>();
            CategoryStats = categories.ToDictionary(
                c => c.CategoryName,
                c => articles.Count(a => a.CategoryId == c.CategoryId)
            );

            var staffList = await _httpClient.GetFromJsonAsync<List<SystemAccount>>("https://localhost:7126/api/SystemAccount")
                ?? new List<SystemAccount>();
            StaffStats = staffList.ToDictionary(
                s => s.AccountName,
                s => articles.Count(a => a.CreatedById == s.AccountId)
            );
        }
    }

}