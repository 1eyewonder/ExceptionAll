using ExceptionAll.Dtos;
using System;
using System.Collections.Generic;

namespace ExceptionAll.Interfaces
{
    public interface IErrorResponseService
    {
        void AddErrorResponse(ErrorResponse response);

        void AddErrorResponses(List<ErrorResponse> responses);

        Dictionary<Type, ErrorResponse> GetErrorResponses();
    }
}