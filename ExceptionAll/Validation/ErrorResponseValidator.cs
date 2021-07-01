using ExceptionAll.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ExceptionAll.Validation
{
    public class ErrorResponseValidator : AbstractValidator<ErrorResponse>
    {
        public ErrorResponseValidator()
        {
            RuleFor(x => x.ExceptionType.BaseType).Equal(typeof(Exception));
            RuleFor(x => x.DetailsType.BaseType).Equal(typeof(ProblemDetails));
        }
    }
}