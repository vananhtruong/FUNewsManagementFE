using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.Categories
{
    [Authorize(Policy = "StaffOnly")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<Category> Category { get; set; } = Enumerable.Empty<Category>();

        public async Task OnGetAsync()
        {
            Category = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7126/api/Category");


        }
    }
}
