using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Tags
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag =  await _httpClient.GetFromJsonAsync<Tag>($"https://localhost:7126/api/Tag/{id}");
            if (tag == null)
            {
                return NotFound();
            }
            Tag = tag;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            

            if (ModelState.IsValid)
            {
                await _httpClient.PutAsJsonAsync($"https://localhost:7126/api/Tag", Tag);
                return RedirectToPage("./Index");
            }

            return Page();
     
        }

     
    }
}
