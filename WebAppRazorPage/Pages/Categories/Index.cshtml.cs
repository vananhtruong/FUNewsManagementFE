using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.Categories
{
    [Authorize(Policy = "StaffOnly")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<Category> Category { get; set; } = Enumerable.Empty<Category>();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            Category = await client.GetFromJsonAsync<List<Category>>("api/Category");
        }
    }
}
