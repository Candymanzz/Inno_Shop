using FluentValidation;
using UserService.Application.Commands.Users;

namespace UserService.Application.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(10);

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MinimumLength(2);
        }
    }
}
