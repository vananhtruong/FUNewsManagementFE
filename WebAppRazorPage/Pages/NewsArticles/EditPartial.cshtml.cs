using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]
    public class EditPartialModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public EditPartialModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public SelectList Categories { get; set; }
        public List<SelectListItem> Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            NewsArticle = await _httpClient.GetFromJsonAsync<NewsArticle>($"https://localhost:7126/api/NewsArticle/{id}");
            if (NewsArticle == null)
            {
                return NotFound();
            }

            SelectedTagIds = NewsArticle.Tags?.Select(t => t.TagId).ToList() ?? new List<int>();

            Categories = new SelectList(
                await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7126/api/Category/active"),
                "CategoryId",
                "CategoryName",
                NewsArticle.CategoryId);

            var allTags = await _httpClient.GetFromJsonAsync<List<Tag>>("https://localhost:7126/api/Tag");
            Tags = allTags
                .Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName })
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Console.WriteLine($"[DEBUG] ID: {id}");
            Console.WriteLine($"[DEBUG] ID: {NewsArticle.NewsArticleId}");
            Console.WriteLine($"TagIds count: {SelectedTagIds?.Count ?? 0}");
            if (id != NewsArticle.NewsArticleId)
            {
                return NotFound();
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdValue) && short.TryParse(userIdValue, out short staffId))
            {
                NewsArticle.UpdatedById = staffId;
            }

            NewsArticle.ModifiedDate = DateTime.Now;
            Console.WriteLine($"[DEBUG] TagIds Count: {SelectedTagIds?.Count ?? 0}");
            Console.WriteLine($"[DEBUG] TagIds: {string.Join(", ", SelectedTagIds ?? new List<int>())}");
            var model = new
            {
                NewsArticle = NewsArticle,
                TagIds = SelectedTagIds
            };
            await _httpClient.PutAsJsonAsync($"https://localhost:7126/api/NewsArticle/v1", model);

            return RedirectToPage("/NewsArticles/Index");
        }
    }
}
