using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;

namespace FUNewsManagementSystem.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var category = await client.GetFromJsonAsync<Category>($"api/Category/{id}");
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }
    }
}
