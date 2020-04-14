using System.Net.Http;
using Foodsharing_app.Exceptions;
using Foodsharing_app.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foodsharing_app.Filters
{
    public class AdminAuthorizationFilter : IActionFilter
    {
        private readonly AuthorizationService _authorizationService;

        public AdminAuthorizationFilter(AuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var authorizationHeader = AuthorizationService.GetAuthorizationHeaderFromFilterContext(context);
            _authorizationService.AuthorizeAdmin(authorizationHeader);
        }
    }

    public class AuthorizedAdmin : ServiceFilterAttribute
    {
        public AuthorizedAdmin() : base(typeof(AdminAuthorizationFilter))
        {
        }
    }
}