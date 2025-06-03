namespace GrpcGreeter
{
    using FluentValidation;

    public class NumberRequestValidator : AbstractValidator<NumberRequest>
    {
        public NumberRequestValidator()
        {
            RuleFor(x => x.Num).NotEmpty().WithMessage("Number is required.");
            RuleFor(x => (int)x.Num).InclusiveBetween(1, 100).WithMessage("Number must be between 1 and 100.");
        }
    }
}