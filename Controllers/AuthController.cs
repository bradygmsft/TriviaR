using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;

namespace TriviaR.Controllers
{
    [Route("/")]
    public class AuthController : Controller
    {
        private string adminPassword;

        public AuthController(IConfiguration configuration)
        {
            adminPassword = configuration["AdminPassword"];
        }

        [HttpGet("Account/Login")]
        public async Task<IActionResult> AdminLogin(string returnUrl, string password)
        {
            if (password != adminPassword) return new UnauthorizedResult();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Redirect(returnUrl ?? "/");
        }
    }
}
