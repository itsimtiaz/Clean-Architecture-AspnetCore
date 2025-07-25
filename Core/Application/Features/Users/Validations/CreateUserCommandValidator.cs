using Application.Features.Users.Commands;
using FluentValidation;

namespace Application.Features.Users.Validations;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(_ => _.Name).Length(3, 20);

        RuleFor(_ => _.Age)
        .GreaterThanOrEqualTo(18)
        .LessThanOrEqualTo(40)
        .WithMessage("{PropertyName} range is between 18 to 40");
    }
}
