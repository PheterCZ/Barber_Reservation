using FluentValidation;
using backend.DTOs;

namespace backend.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(user => user.Id)
                .NotEmpty().WithMessage("ID uživatele je povinné.");
            RuleFor(user => user.FullName)
                .NotEmpty().WithMessage("Celé jméno je povinné.");
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("E-mail je povinný.")
                .EmailAddress().WithMessage("Zadejte platný e-mailový formát.");
            RuleFor(user => user.Roles)
                .NotEmpty().WithMessage("Musí být vybrána alespoň jedna role.");
        }
    }
}