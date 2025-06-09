using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazorPage.Model;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(HttpClient httpClient, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var defaultAdminEmail = _config["DefaultAccount:Email"];
            var defaultAdminPassword = _config["DefaultAccount:Password"];

            if (Input.Email == defaultAdminEmail && Input.Password == defaultAdminPassword)
            {
                var adminClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Default Admin"),
                    new Claim(ClaimTypes.Email, defaultAdminEmail),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var adminIdentity = new ClaimsIdentity(adminClaims, "MyCookieAuth");
                var adminrincipal = new ClaimsPrincipal(adminIdentity);
                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(adminIdentity));

                return RedirectToPage("/SystemAccounts/Index");
            }

            var loginModel = new
            {
                Email = Input.Email,
                Password = Input.Password
            };
            var client = _httpClientFactory.CreateClient("MyApi");

            var response = await client.PostAsJsonAsync("api/SystemAccount/Login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize user data from response
                var user = await response.Content.ReadFromJsonAsync<SystemAccount>();

                var role = user.AccountRole switch
                {
                    1 => "Staff",
                    2 => "Lecturer",
                    _ => "Lecturer"
                };

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                    new Claim(ClaimTypes.Name, user.AccountName ?? ""),
                    new Claim(ClaimTypes.Email, user.AccountEmail ?? ""),
                    new Claim(ClaimTypes.Role, role)
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal);

                return role switch
                {
                    "Staff" => RedirectToPage("/NewsArticles/Index"),
                    "Lecturer" => RedirectToPage("/NewsArticles/LecturerNews"),
                    _ => RedirectToPage("/NewsArticles/LecturerNews")
                };
            }

            ErrorMessage = "Invalid credentials!";
            return Page(); // Quay lại trang login
        }

        public class LoginInputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
