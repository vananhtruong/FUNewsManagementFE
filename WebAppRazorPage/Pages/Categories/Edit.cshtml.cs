using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities;

namespace FUNewsManagementSystem.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApi");

            var category = await client.GetFromJsonAsync<Category>($"api/Category/{id}");
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            var activeCategories = await client.GetFromJsonAsync<List<Category>>("api/Category/active");
            ViewData["ParentCategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            await client.PutAsJsonAsync("api/Category", Category);
            return RedirectToPage("./Index");
        }
    }
}
