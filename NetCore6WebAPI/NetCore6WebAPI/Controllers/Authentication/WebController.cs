using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NetCore6APIDataTransfer.Requests.Authentication;
using static NetCore6APIDataTransfer.ApiRoutes;
using Microsoft.AspNetCore.Authorization;

namespace NetCore6WebAPI.Controllers.Authentication
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    [ApiController]
    [Route("")]
    public class WebController : ControllerBase
    {
        private readonly ILogger<WebController> _Logger;
        private IConfiguration _Configuration;
        private IAntiforgery _Antiforgery;

        public WebController(ILogger<WebController> logger, IConfiguration configuration, IAntiforgery antiforgery)
        {
            _Logger = logger;
            _Configuration = configuration;
            _Antiforgery = antiforgery;
        }

        [AllowAnonymous]
        [HttpPost(WebAuthenticationRoute.PostWebLoginInformation)]
        public async Task<IActionResult> RequestCookie([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest.UserName == "Test" && tokenRequest.Password == "Password")
            {

                var claims = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, tokenRequest.UserName)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var userIdentity = new ClaimsPrincipal(claims);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userIdentity);

                return Ok(new
                {
                    userName = tokenRequest.UserName,
                    firstName = "Test",
                    lastName = "Tester",
                    fingetPrint = Guid.NewGuid(),
                });
            }

            return BadRequest("Could not verify username and password");
        }

        [Authorize]
        [HttpGet(WebAuthenticationRoute.GetWebLoginAntiForgeryCookie)]
        public IActionResult RequesAntiForgeryCookie()
        {
            var tokens = _Antiforgery.GetAndStoreTokens(HttpContext);
            CookieOptions CookieOptions = new CookieOptions();
            CookieOptions.HttpOnly = false;
            CookieOptions.SameSite = SameSiteMode.Strict;

            HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, CookieOptions);

            return Ok();
        }

        [HttpGet(WebAuthenticationRoute.PostWebLogoutInformation)]
        public string Logout()
        {
            return "Logout Method";
        }
    }
}
