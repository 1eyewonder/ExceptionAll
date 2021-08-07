using System;
using ExceptionAll.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExceptionAll.Filters
{
    /// <summary>
    /// Catches exceptions and creates standard responses using the IActionResultService
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IActionResultService _actionResultService;

        public ExceptionFilter(IActionResultService actionResultService)
        {
            _actionResultService = actionResultService ?? throw new ArgumentNullException(nameof(actionResultService));
        }

        public override void OnException(ExceptionContext context)
        {
            context.Result = _actionResultService.GetErrorResponse(context);
        }
    }
}