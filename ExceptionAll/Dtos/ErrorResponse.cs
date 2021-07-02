using System;

namespace ExceptionAll.Dtos
{
    public class ErrorResponse
    {
        public string ErrorTitle { get; set; } = "Default Title";
        public Type ExceptionType { get; set; } = typeof(Exception);
        public Type DetailsType { get; set; }
        public Action<Exception> LogAction { get; set; } = null;
    }
}