using ExceptionAll.Details;
using ExceptionAll.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace ExceptionAll.Dtos
{
    public interface IErrorResponse
    {
        Type DetailsType { get; }
        string ErrorTitle { get; }
        Type ExceptionType { get; }
        Action<ILogger<IActionResultService>, Exception> LogAction { get; }
    }

    public interface IDetailsType : IErrorResponse
    {
        public ILogAction WithReturnType(Type returnType);
    }

    public interface IExceptionSelection : IErrorResponse
    {
        public IDetailsType ForException(Type exceptionType);
    }

    public interface ILogAction : IErrorResponse
    {
        public ErrorResponse WithLogAction(Action<ILogger<IActionResultService>, Exception> action);
    }

    public interface IResponseTitle : IErrorResponse
    {
        public IExceptionSelection WithTitle(string title);
    }

    public class ErrorResponse : IResponseTitle,
        IExceptionSelection,
        IDetailsType,
        ILogAction
    {
        private readonly IActionResultService _actionResultService;

        private ErrorResponse(IActionResultService actionResultService)
        {
            _actionResultService = actionResultService ?? throw new ArgumentNullException(nameof(actionResultService));
        }

        public ILogAction WithReturnType(Type returnType)
        {
            DetailsType = returnType;
            return this;
        }

        public IDetailsType ForException(Type exceptionType)
        {
            ExceptionType = exceptionType;
            return this;
        }

        ErrorResponse ILogAction.WithLogAction(Action<ILogger<IActionResultService>, Exception> action)
        {
            LogAction = action;
            return this;
        }

        public Type DetailsType { get; private set; } = typeof(InternalServerErrorDetails);
        public string ErrorTitle { get; private set; } = "Error";
        public Type ExceptionType { get; private set; } = typeof(Exception);

        public Action<ILogger<IActionResultService>, Exception> LogAction { get; private set; } =
            (x, e) => x.LogError(e, "ExceptionAll has caught an error has occurred");

        public IExceptionSelection WithTitle(string title)
        {
            ErrorTitle = title;
            return this;
        }

        public static ErrorResponse CreateErrorResponse(IActionResultService actionResultService)
        {
            return new ErrorResponse(actionResultService);
        }
    }
}