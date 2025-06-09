using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var random = new Random();
            // Tạo tài khoản role luôn là Lecturer (2)
            var newAccount = new SystemAccount
            {
                AccountId = (short)random.Next(short.MinValue, short.MaxValue + 1),
                AccountName = Input.AccountName,
                AccountEmail = Input.AccountEmail,
                AccountPassword = Input.AccountPassword,
                AccountRole = 2 // 2 là Lecturer
            };

            try
            {
                var client = _httpClientFactory.CreateClient("MyApi");
                var response = await client.PostAsJsonAsync("api/SystemAccount", newAccount);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Đăng ký thành công!";
                    return RedirectToPage("/SystemAccounts/Login");
                }
                else
                {
                    ErrorMessage = "Register failed! Email có thể đã tồn tại.";
                    return Page();
                }
            }
            catch
            {
                ErrorMessage = "Có lỗi hệ thống. Vui lòng thử lại sau.";
                return Page();
            }
        }

        public class RegisterInputModel
        {
            public string AccountName { get; set; }
            public string AccountEmail { get; set; }
            public string AccountPassword { get; set; }
        }
    }
}
