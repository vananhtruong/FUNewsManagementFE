using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebAppRazorPage
{
    public static class ServiceCollectionExtension
    {
        public static void AddAuthen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "MyCookieAuth";
                options.DefaultChallengeScheme = "Google"; // Khi bấm login Google sẽ dùng scheme này
            })
            .AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyAuthCookie";
                options.LoginPath = "/SystemAccounts/Login";
                options.AccessDeniedPath = "/Error";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddGoogle("Google", options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"];
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff"));
                options.AddPolicy("LecturerOnly", policy => policy.RequireRole("Lecturer"));
            });
        }

    }
}
