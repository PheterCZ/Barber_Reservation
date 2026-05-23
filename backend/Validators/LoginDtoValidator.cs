using backend.DTOs;
using FluentValidation;

namespace backend.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail je povinný.")
                .EmailAddress().WithMessage("Zadejte platný e-mailový formát.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Heslo je povinné.");
        }
    }
}