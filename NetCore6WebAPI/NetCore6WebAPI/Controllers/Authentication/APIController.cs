using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetCore6APIDataTransfer.Requests.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static NetCore6APIDataTransfer.ApiRoutes;

namespace NetCore6WebAPI.Controllers.Authentication
{
    [ApiController]
    [Route("")]
    public class APIController : ControllerBase
    {
        private readonly ILogger<APIController> _Logger;
        private IConfiguration _Configuration;

        public APIController(ILogger<APIController> logger, IConfiguration configuration)
        {
            _Logger = logger;
            _Configuration = configuration;
        }

        [HttpPost(APIAuthenticationRoute.PostAPILoginInformation)]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (request.UserName == "Test" && request.Password == "Password")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT:IssuerSigningKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _Configuration["JWT:ValidIssuer"],
                    audience: _Configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                //TODO: Add identity information to the fingerprint (i.e. server info, ect.)
                var fingerPrint = Guid.NewGuid();

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    fingerPrint
                });
            }

            return BadRequest("Could not verify username and password");
        }
    }
}
