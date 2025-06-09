using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public SystemAccount SystemAccount { get; set; }

        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public EditProfileModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !short.TryParse(userIdClaim, out short userId))
            {
                return Unauthorized();
            }

            var account = await client.GetFromJsonAsync<SystemAccount>($"api/SystemAccount/{userId}");
            if (account == null) return NotFound();

            SystemAccount = account;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !short.TryParse(userIdClaim, out short userId))
            {
                return Unauthorized();
            }

            if (userId != SystemAccount.AccountId) return Forbid();

            var existingAccount = await client.GetFromJsonAsync<SystemAccount>($"api/SystemAccount/{userId}");
            if (existingAccount == null) return NotFound();

            // Kiểm tra nếu người dùng nhập mật khẩu cũ và mới
            if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
            {
                if (existingAccount.AccountPassword != OldPassword)
                {
                    TempData["ErrorMessage"] = "Old password is incorrect!";
                    return Page();
                }
                existingAccount.AccountPassword = NewPassword;
            }

            existingAccount.AccountName = SystemAccount.AccountName;

            await client.PutAsJsonAsync($"api/SystemAccount", existingAccount);
            TempData["SuccessMessage"] = "Profile updated successfully!";

            return RedirectToPage("/SystemAccounts/EditProfile");
        }
    }
}
