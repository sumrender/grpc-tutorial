namespace GrpcGreeter
{
    using FluentValidation;

    public class HelloRequestValidator : AbstractValidator<HelloRequest>
    {
        public HelloRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        }
    }
}