using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities;

namespace FUNewsManagementSystem.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  await _httpClient.GetFromJsonAsync<Category>($"https://localhost:7126/api/Category/{id}");
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            var activeCategories = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7126/api/Category/active");
            ViewData["ParentCategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            await _httpClient.PutAsJsonAsync("https://localhost:7126/api/Category", Category);
            return RedirectToPage("./Index");
        }

       
    }
}
