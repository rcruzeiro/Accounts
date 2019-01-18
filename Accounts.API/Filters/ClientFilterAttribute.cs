using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Accounts.API.Filters
{
    public class ClientFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("client") ||
                 string.IsNullOrEmpty(context.ActionArguments["client"].ToString()))
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult
                {
                    Content = "The client field is required.",
                    StatusCode = 400
                };
                return;
            }


            base.OnActionExecuting(context);
        }
    }
}
