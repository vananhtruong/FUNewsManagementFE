using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("MyApi");

                // Lấy TagId lớn nhất và +1
                var maxId = (await client.GetFromJsonAsync<int?>("api/Tag/MaxId")) ?? 0;
                Tag.TagId = maxId + 1;

                await client.PostAsJsonAsync("api/Tag", Tag);

                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
