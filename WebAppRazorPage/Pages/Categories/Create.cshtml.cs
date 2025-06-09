using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities;

namespace FUNewsManagementSystem.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var activeCategories = await client.GetFromJsonAsync<List<Category>>("api/Category/active");
            ViewData["ParentCategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryName");
            return Page();
        }


        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");

            if (!ModelState.IsValid)
            {
                var parentCategories = await client.GetFromJsonAsync<List<Category>>("api/Category");
                ViewData["ParentCategoryId"] = new SelectList(parentCategories, "CategoryId", "CategoryName", Category.ParentCategoryId);
                return Page();
            }

            await client.PostAsJsonAsync("api/Category", Category);
            return RedirectToPage("/Categories/Index");
        }


    }
}
