using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IActionResult> OnPostAsync(string id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7126/api/NewsArticle/{id}");
            return RedirectToPage("/NewsArticles/Index");
        }
    }
}
