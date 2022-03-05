using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static NetCore6APIDataTransfer.ApiRoutes;

namespace NetCore6WebAPI.Controllers.Authentication
{
    [ApiController]
    [Route("")]
    public class APIController : ControllerBase
    {
        private readonly ILogger<APIController> _logger;

        public APIController(ILogger<APIController> logger)
        {
            _logger = logger;
        }

        [HttpGet(APIAuthenticationRoute.PostAPILoginInformation)]
        public string Login()
        {
            return "login Method";
        }
    }
}
