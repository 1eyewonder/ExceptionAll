using ExceptionAll.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExceptionAll.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        private readonly IActionResultService _actionResultService;

        public ExceptionFilter(IActionResultService actionResultService)
        {
            _actionResultService = actionResultService;
        }

        public override void OnException(ExceptionContext context)
        {
            context.Result = _actionResultService.GetErrorResponse(context);
        }
    }
}