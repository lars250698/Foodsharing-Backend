using System.Net.Http;
using Foodsharing_app.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foodsharing_app.Filters
{
    public class AdminAuthorizationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}