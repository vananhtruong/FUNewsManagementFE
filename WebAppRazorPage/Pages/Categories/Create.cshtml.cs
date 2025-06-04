using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities;

namespace FUNewsManagementSystem.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var activeCategories = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7126/api/Category/active");
            ViewData["ParentCategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var parentCategories = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7126/api/Category");
                ViewData["ParentCategoryId"] = new SelectList(parentCategories, "CategoryId", "CategoryName", Category.ParentCategoryId);
                return Page();
            }

            await _httpClient.PostAsJsonAsync("https://localhost:7126/api/Category", Category);
            return RedirectToPage("/Categories/Index");
        }

    }
}
