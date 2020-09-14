using Application.Exceptions.BadRequest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Exceptions;

namespace WebApi.Filters
{
    public class BadRequestExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is FluentValidationException fluentValidationException)
            {
                SetResult(context, new BadRequestObjectResult(fluentValidationException.Errors));
                ResetException(context);
            }

            if (!(context.Exception is BadRequestException badRequestException)) return;

            SetResult(context, new BadRequestObjectResult(badRequestException.Response));
            ResetException(context);
        }

        private static void ResetException(ExceptionContext context)
        {
            context.Exception = null;
        }

        private static void SetResult(ExceptionContext context, IActionResult result)
        {
            context.Result = result;
        }
    }
}