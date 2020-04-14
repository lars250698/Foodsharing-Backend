using Foodsharing_app.Filters;
using Foodsharing_app.Services;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly AuthorizationService _authorizationService;

        public AuthorizationController(AuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("login")]
        public string Login([FromQuery] string username, [FromQuery] string password) =>
            _authorizationService.Login(username, password);
        
        [HttpGet]
        [Route("authToken")]
        public string GetAuthToken([FromQuery] string refreshToken) =>
            _authorizationService.IssueAuthToken(refreshToken);
    }
}