using ExceptionAll.Dtos;
using System;
using System.Collections.Generic;

namespace ExceptionAll.Interfaces
{
    /// <summary>
    /// Service for managing global error responses
    /// </summary>
    public interface IErrorResponseService
    {
        /// <summary>
        /// Add a standard error response for a specific exception type
        /// </summary>
        /// <param name="response"></param>
        void AddErrorResponse(ErrorResponse response);

        /// <summary>
        /// Return all error responses in the current collection
        /// </summary>
        /// <returns></returns>
        Dictionary<Type, ErrorResponse> GetErrorResponses();
    }
}