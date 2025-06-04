using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;


namespace FUNewsManagementSystem.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Lấy TagId lớn nhất và +1
                var maxId = (await _httpClient.GetFromJsonAsync<int?>("https://localhost:7126/api/Tag/MaxId")) ?? 0;
                Tag.TagId = maxId + 1;

                await _httpClient.PostAsJsonAsync("https://localhost:7126/api/Tag", Tag);

                return RedirectToPage("./Index");
            }

            return Page();
          
        }
    }
}
