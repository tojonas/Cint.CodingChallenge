using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Cint.CodingChallenge.Web.Filters
{
    public class ModelStateValidationFilter : IActionFilter
    {
        private readonly ILogger _logger;
        public ModelStateValidationFilter(ILogger<ModelStateValidationFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }
            _logger.LogWarning("ModelState validation failed for the following fields:");

            foreach (var (key, value) in context.ModelState)
            {
                foreach (var error in value.Errors)
                {
                    _logger.LogWarning($"{key}: {error.ErrorMessage}");
                }
            }
            context.Result = new BadRequestObjectResult("ModelState validation failed.");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
