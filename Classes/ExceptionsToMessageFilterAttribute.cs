using pujcovna.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace pujcovna.Classes
{
    public class ExceptionsToMessageFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
                return;

            ((Controller)context.Controller).AddDebugMessage(context.Exception);

            string referrer = context.HttpContext.Request.Headers["Referer"];
            context.Result = !string.IsNullOrEmpty(referrer)
                ? new RedirectResult(referrer)
                : (IActionResult)new RedirectToActionResult("Index", "Home", null);

            context.ExceptionHandled = true;
        }
    }
}