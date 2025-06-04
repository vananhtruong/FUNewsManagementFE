using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http;
using System.Security.Claims;


namespace FUNewsManagementSystem.Pages.NewsArticles
{


    public class CreatePartialModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> TagIds { get; set; } = new();

        public SelectList CategoryList { get; set; }
        public MultiSelectList TagList { get; set; }

        public CreatePartialModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        [Authorize(Policy = "StaffOnly")]
        public async Task<IActionResult> OnGetAsync()
        {
            CategoryList = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7126/api/Category/active"), "CategoryId", "CategoryName");
            var tags = await _httpClient.GetFromJsonAsync<List<Tag>>("https://localhost:7126/api/Tag");
            TagList = new MultiSelectList(tags, "TagId", "TagName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
        

            if (string.IsNullOrEmpty(NewsArticle.NewsArticleId))
            {
                NewsArticle.NewsArticleId = Guid.NewGuid().ToString("N").Substring(0, 20);
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdValue) && short.TryParse(userIdValue, out short staffId))
            {
                NewsArticle.CreatedById = staffId;
                NewsArticle.UpdatedById = staffId;
            }

            NewsArticle.CreatedDate = DateTime.Now;
            NewsArticle.ModifiedDate = DateTime.Now;

            var model = new
            {
                NewsArticle = NewsArticle,
                TagIds = TagIds
            };
            await _httpClient.PostAsJsonAsync("https://localhost:7126/api/NewsArticle", model);
            return RedirectToPage("/NewsArticles/Index");
        }


    }
}