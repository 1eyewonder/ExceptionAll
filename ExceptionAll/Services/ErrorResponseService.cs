using ExceptionAll.Dtos;
using ExceptionAll.Interfaces;
using ExceptionAll.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExceptionAll.Services
{
    public class ErrorResponseService : IErrorResponseService
    {
        private Dictionary<Type, ErrorResponse> ErrorResponses { get; set; } = new();

        public void AddErrorResponse(ErrorResponse response)
        {
            new ErrorResponseValidator().ValidateAndThrow(response);
            ErrorResponses.Add(response.ExceptionType, response);
        }

        public void AddErrorResponses(List<ErrorResponse> responses)
        {
            if (responses is null || !responses.Any())
            {
                throw new ArgumentNullException(nameof(responses));
            }

            foreach (var response in responses)
            {
                AddErrorResponse(response);
            }
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