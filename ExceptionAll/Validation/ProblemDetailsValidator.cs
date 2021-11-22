namespace ExceptionAll.Validation;

public class ProblemDetailsValidator<T> : AbstractValidator<T> where T : BaseDetails
{
    public ProblemDetailsValidator()
    {
        RuleFor(x => x.Status).NotNull();
        RuleFor(x => x.Status).GreaterThanOrEqualTo(100);
        RuleFor(x => x.Status).LessThan(600);
        RuleFor(x => x.Title).NotNull();
        RuleFor(x => x.Title).NotEmpty();
    }
}
