using System;

namespace ExceptionAll.Dtos
{
    public class ErrorResponse
    {
        public string ErrorTitle { get; init; } = "Default Title";
        public Type ExceptionType { get; init; } = typeof(Exception);
        public Type DetailsType { get; init; }
        public Action<Exception> LogAction { get; init; }
    }
}