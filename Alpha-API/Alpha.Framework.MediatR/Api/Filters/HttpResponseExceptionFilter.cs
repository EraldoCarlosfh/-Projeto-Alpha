using Alpha.Framework.MediatR.EventSourcing.Validators;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Api.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;       

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var messageSB = new StringBuilder();

                var erroneousFields =
                    context.ModelState
                        .Where(ms => ms.Value.Errors.Any())
                        .Select(x => new
                        {
                            Field = x.Key,
                            Description = GetErrorsInline(x.Value.Errors)
                        });

                foreach (var item in erroneousFields)
                {
                    messageSB.AppendLine($"{item.Field} - {item.Description}");
                }

                context.Result = new BadRequestObjectResult(messageSB.ToString());
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var statusCode = 500;
                var message = context.Exception.Message;

                if (!string.IsNullOrEmpty(context.Exception?.InnerException?.Message))
                    message = $"{message} - {context.Exception?.InnerException?.Message}";

                if (context.Exception is BusinessException exception)
                {
                    statusCode = exception.StatusCode;
                    message = exception.Message;
                }

                context.Result = new ObjectResult(message)
                {
                    StatusCode = statusCode
                };
                context.ExceptionHandled = true;

            }
        }

        private static string GetErrorsInline(ModelErrorCollection errors)
        {
            var errorList = errors.Select(c => c.ErrorMessage).ToList();
            return string.Join(",", errorList);
        }

    }
}
