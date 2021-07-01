using ExceptionAll.Dtos;
using ExceptionAll.Interfaces;
using System;
using System.Collections.Generic;

namespace ExceptionAll.Services
{
    public class ErrorResponseService : IErrorResponseService
    {
        private Dictionary<Type, ErrorResponse> ErrorResponses { get; set; } = new Dictionary<Type, ErrorResponse>();

        public void AddErrorResponse(ErrorResponse response)
        {
            ErrorResponses.Add(response.ExceptionType, response);
        }

        public void ClearErrorResponses()
        {
            ErrorResponses = new Dictionary<Type, ErrorResponse>();
        }

        public Dictionary<Type, ErrorResponse> GetErrorResponses()
        {
            return ErrorResponses;
        }
    }
}