using ExceptionAll.Dtos;
using System;
using System.Collections.Generic;

namespace ExceptionAll.Interfaces
{
    public interface IErrorResponseService
    {
        void AddErrorResponse(ErrorResponse response);

        void AddErrorResponses(List<ErrorResponse> responses);

        void ClearErrorResponses();

        Dictionary<Type, ErrorResponse> GetErrorResponses();
    }
}