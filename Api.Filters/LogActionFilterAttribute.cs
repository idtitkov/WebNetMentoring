using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Filters
{
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        private readonly bool _logParameters;
        private ILogger<LogActionFilterAttribute> _logger;

        public LogActionFilterAttribute(bool logParameters = true)
        {
            _logParameters = logParameters;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger = context.HttpContext.RequestServices.GetService<ILogger<LogActionFilterAttribute>>();
            var controllerName = context.Controller.GetType().Name;

            _logger.LogInformation($"{controllerName} started");
            if (_logParameters)
            {
                _logger.LogInformation($"{controllerName} parameters: {string.Join(" ", context.ActionArguments)}");
            }

            await base.OnActionExecutionAsync(context, next);
            _logger.LogInformation($"{context.Controller.GetType().Name} ended");
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }
    }
}
