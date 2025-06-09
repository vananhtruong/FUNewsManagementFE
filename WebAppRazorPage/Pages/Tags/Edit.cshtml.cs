using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Tags
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApi");
            var tag = await client.GetFromJsonAsync<Tag>($"api/Tag/{id}");
            if (tag == null)
            {
                return NotFound();
            }
            Tag = tag;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");

            if (ModelState.IsValid)
            {
                await client.PutAsJsonAsync("api/Tag", Tag);
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
