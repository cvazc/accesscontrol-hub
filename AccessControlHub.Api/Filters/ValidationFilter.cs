using AccessControlHub.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccessControlHub.Api.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var firstError = context.ModelState
                .Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault();

            if (firstError != null)
            {
                throw new ValidationException(firstError.ErrorMessage);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Nothing to do here
    }
}