using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync(short id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7126/api/Category/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Categories/Index");
            }
            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(error);
        
        }

    }
}
