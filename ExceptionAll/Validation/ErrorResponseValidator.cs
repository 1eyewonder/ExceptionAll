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
            RuleFor(x => x.ExceptionType)
                .Must(x => x.IsSubclassOf(typeof(Exception)))
                .When(x => x.ExceptionType != typeof(Exception));

            RuleFor(x => x.ExceptionType).NotNull();

            RuleFor(x => x.DetailsType)
                .Must(x => x.IsSubclassOf(typeof(ProblemDetails)));

            RuleFor(x => x.DetailsType).NotNull();

            RuleFor(x => x.ErrorTitle).NotEmpty();
        }
    }
}