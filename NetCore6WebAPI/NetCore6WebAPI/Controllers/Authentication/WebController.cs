using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static NetCore6APIDataTransfer.ApiRoutes;

namespace NetCore6WebAPI.Controllers.Authentication
{
    [ApiController]
    [Route("")]
    public class WebController : ControllerBase
    {
        private readonly ILogger<WebController> _logger;

        public WebController(ILogger<WebController> logger)
        {
            _logger = logger;
        }

        [HttpGet(WebAuthenticationRoute.PostWebLoginInformation)]
        public string Login()
        {
            return "login Method";
        }

        [HttpGet(WebAuthenticationRoute.PostWebLogoutInformation)]
        public string Logout()
        {
            return "Logout Method";
        }

        [HttpGet(WebAuthenticationRoute.PostWebLoginAntiForgeryCookie)]
        public string AntiForgeryCookie()
        {
            return "Antiforgery Cookie";
        }
    }
}
