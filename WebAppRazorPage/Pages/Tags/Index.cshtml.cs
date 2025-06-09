using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            Tags = await client.GetFromJsonAsync<List<Tag>>("api/Tag");
        }
    }
}
