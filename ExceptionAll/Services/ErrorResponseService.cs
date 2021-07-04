using ExceptionAll.Dtos;
using ExceptionAll.Interfaces;
using ExceptionAll.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ExceptionAll.Services
{
    public class ErrorResponseService : IErrorResponseService
    {
        private readonly ILogger<IErrorResponseService> _logger;
        private Dictionary<Type, ErrorResponse> ErrorResponses { get; } = new();

        public ErrorResponseService(ILogger<IErrorResponseService> logger)
        {
            _logger = logger;
        }

        public void AddErrorResponse(ErrorResponse response)
        {
            new ErrorResponseValidator().ValidateAndThrow(response);
            if (ErrorResponses.ContainsKey(response.ExceptionType))
            {
                _logger.LogError("Cannot add response to service because an " +
                                   $"error response already exists for this exception type: {response.ExceptionType}");
                throw new ArgumentException($"Exception type, {response.ExceptionType}, " +
                                            "already exists in service collection");
            }

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

        public Dictionary<Type, ErrorResponse> GetErrorResponses()
        {
            return ErrorResponses;
        }
    }
}