using System;

namespace ExceptionAll.Dtos
{
    public class ErrorResponse
    {
        public string ErrorTitle { get; set; }
        public Type ExceptionType { get; set; }
        public Type DetailsType { get; set; }
        public Action<Exception> LogAction { get; set; } = null;
        public bool ExceptionVisibleToUser { get; set; } = false;
    }
}