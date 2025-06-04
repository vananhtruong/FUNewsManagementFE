using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();

        public async Task OnGetAsync()
        {
            Tags = await _httpClient.GetFromJsonAsync<List<Tag>>("https://localhost:7126/api/Tag");


        }
    }
}
