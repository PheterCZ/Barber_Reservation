using FluentValidation;
using backend.DTOs;

namespace backend.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(register => register.FirstName)
                .NotEmpty().WithMessage("Jméno je povinné.")
                .MaximumLength(50).WithMessage("Jméno může mít maximálně 50 znaků.");
            
            RuleFor(register => register.LastName)
                .NotEmpty().WithMessage("Příjmení je povinné.")
                .MaximumLength(50).WithMessage("Příjmení může mít maximálně 50 znaků.");

            RuleFor(register => register.Email)
                .NotEmpty().WithMessage("E-mail je povinný.")
                .EmailAddress().WithMessage("Zadejte platný e-mailový formát.");

            RuleFor(register => register.Password)
                .NotEmpty().WithMessage("Heslo je povinné.")
                .MinimumLength(6).WithMessage("Heslo musí mít alespoň 6 znaků.")
                .MaximumLength(100).WithMessage("Heslo může mít maximálně 100 znaků.")
                .Matches(@"[A-Z]").WithMessage("Heslo musí obsahovat alespoň jedno velké písmeno.")
                .Matches(@"[a-z]").WithMessage("Heslo musí obsahovat alespoň jedno malé písmeno.")
                .Matches(@"[0-9]").WithMessage("Heslo musí obsahovat alespoň jednu číslici.");

            RuleFor(register => register.Phone)
                .NotEmpty().WithMessage("Telefonní číslo je povinné.")
                .MaximumLength(20).WithMessage("Telefonní číslo může mít maximálně 20 znaků.")
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Zadejte telefonní číslo ve správném formátu.");
        }
    }
}