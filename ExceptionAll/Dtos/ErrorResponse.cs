using ExceptionAll.Details;
using ExceptionAll.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ExceptionAll.Dtos
{
    public class ErrorResponse : IResponseTitle,
        IExceptionSelection,
        IDetailsType,
        ILogAction
    {
        public Type DetailsType { get; private set; } = typeof(InternalServerErrorDetails);
        public string ErrorTitle { get; private set; } = "Error";
        public Type ExceptionType { get; private set; } = typeof(Exception);
        public Action<ILogger<IActionResultService>, Exception> LogAction { get; private set; } =
            (x, e) => x.LogDebug(e, "ExceptionAll has caught an error");

        private ErrorResponse()
        {
        }

        public static ErrorResponse CreateErrorResponse()
        {
            return new ErrorResponse();
        }

        public IDetailsType ForException<T>() where T : Exception
        {
            ExceptionType = typeof(T);
            return this;
        }

        ErrorResponse ILogAction.WithLogAction(Action<ILogger<IActionResultService>, Exception> action)
        {
            LogAction = action;
            return this;
        }

        public ILogAction WithReturnType<T>() where T : ProblemDetails
        {
            DetailsType = typeof(T);
            return this;
        }

        public IExceptionSelection WithTitle(string title)
        {
            ErrorTitle = title;
            return this;
        }
    }
}