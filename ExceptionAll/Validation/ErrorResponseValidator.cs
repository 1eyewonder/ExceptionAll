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
            RuleFor(x => x.ExceptionType).NotNull();
            RuleFor(x => x.ExceptionType.BaseType).Equal(typeof(Exception));
            RuleFor(x => x.DetailsType).NotNull();
            RuleFor(x => x.DetailsType.BaseType).Equal(typeof(ProblemDetails));
            RuleFor(x => x.ErrorTitle).NotEmpty();
        }
    }
}