using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]
    public class EditPartialModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditPartialModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public SelectList Categories { get; set; }
        public List<SelectListItem> Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");

            NewsArticle = await client.GetFromJsonAsync<NewsArticle>($"api/NewsArticle/{id}");
            if (NewsArticle == null)
            {
                return NotFound();
            }

            SelectedTagIds = NewsArticle.Tags?.Select(t => t.TagId).ToList() ?? new List<int>();

            Categories = new SelectList(
                await client.GetFromJsonAsync<List<Category>>("api/Category/active"),
                "CategoryId",
                "CategoryName",
                NewsArticle.CategoryId);

            var allTags = await client.GetFromJsonAsync<List<Tag>>("api/Tag");
            Tags = allTags
                .Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName })
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");

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

            var model = new
            {
                NewsArticle = NewsArticle,
                TagIds = SelectedTagIds
            };
            await client.PutAsJsonAsync($"api/NewsArticle/v1", model);

            return RedirectToPage("/NewsArticles/Index");
        }
    }
}
