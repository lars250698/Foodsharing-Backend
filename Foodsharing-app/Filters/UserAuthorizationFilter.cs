using Foodsharing_app.Exceptions;
using Foodsharing_app.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foodsharing_app.Filters
{
    public class UserAuthorizationFilter : IActionFilter
    {
        private readonly AuthorizationService _authorizationService;

        public UserAuthorizationFilter(AuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var authenticationHeader = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authenticationHeader))
            {
                throw new UserNotAuthorizedException("No Authorization header found in request");
            }

            _authorizationService.AuthorizeUser(authenticationHeader);
        }
    }
}