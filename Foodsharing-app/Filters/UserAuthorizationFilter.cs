using System;
using Foodsharing_app.Exceptions;
using Foodsharing_app.Services;
using Microsoft.AspNetCore.Mvc;
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
            var authorizationHeader = AuthorizationService.GetAuthorizationHeaderFromFilterContext(context);
            _authorizationService.AuthorizeUser(authorizationHeader);
        }
    }
    
    public class AuthorizedUser : ServiceFilterAttribute
    {
        public AuthorizedUser() : base(typeof(UserAuthorizationFilter))
        {
        }
    }
}