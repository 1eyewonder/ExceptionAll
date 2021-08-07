using Microsoft.Extensions.Logging;
using System;
using ExceptionAll.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionAll.Interfaces
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
        /// <summary>
        /// Object returned from handling our error 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ILogAction WithReturnType<T>() where T : ProblemDetails;
    }

    public interface IExceptionSelection : IErrorResponse
    {
        /// <summary>
        /// Type of error that will trigger our filter
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <returns></returns>
        public IDetailsType ForException<T>() where T : Exception;
    }

    public interface ILogAction : IErrorResponse
    {
        /// <summary>
        /// Logging action that will occur if an exception is caught
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public ErrorResponse WithLogAction(Action<ILogger<IActionResultService>, Exception> action);
    }

    public interface IResponseTitle : IErrorResponse
    {
        /// <summary>
        /// Creates title for the returned error response object
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IExceptionSelection WithTitle(string title);
    }
}
