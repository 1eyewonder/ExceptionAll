using ExceptionAll.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ExceptionAll.Interfaces
{
    public interface IErrorResponseService
    {
        void AddErrorResponse(ErrorResponse response);

        void ClearErrorResponses();

        Dictionary<Type, ErrorResponse> GetErrorResponses();
    }
}