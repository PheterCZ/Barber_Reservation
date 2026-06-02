using FluentValidation;
using backend.DTOs;
using System.Data;

namespace backend.Validators
{
    public class BarberDtoValidator : AbstractValidator<BarberDto>
    {
        public BarberDtoValidator()
        {
            RuleFor(barber => barber.FirstName)
                .NotEmpty()
                .WithMessage("Jméno je povinné.")
                .MaximumLength(50)
                .WithMessage("Jméno může mít maximálně 50 znaků.");

            RuleFor(barber => barber.LastName)
                .NotEmpty()
                .WithMessage("Příjmení je povinné.")
                .MaximumLength(50)
                .WithMessage("Příjmení může mít maximálně 50 znaků.");

            RuleFor(barber => barber.Phone)
                .NotEmpty()
                .WithMessage("Telefonní číslo je povinné.")
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Zadejte telefonní číslo ve správném formátu.");
                
            RuleFor(barber => barber.Email)
                .NotEmpty()
                .WithMessage("E-mail je povinný.")
                .EmailAddress()
                .WithMessage("Zadejte platný e-mailový formát.");
                
            RuleFor(barber => barber.Specialization)
                .NotEmpty().WithMessage("Specializace je povinná.")
                .MaximumLength(200)
                .WithMessage("Specializace může mít maximálně 200 znaků.");

            RuleFor(barber => barber.Services)
                .NotEmpty()
                .WithMessage("Zadejte alespoň jednu službu.");

            RuleForEach(barber => barber.Services)
                .NotEmpty()
                .WithMessage("Název služby nesmí být prázdný.")
                .MaximumLength(100)
                .WithMessage("Název služby může mít maximálně 100 znaků.");
                
            RuleFor(barber => barber.StartWork)
                .NotEmpty()
                .WithMessage("Datum nástupu do práce je povinné.")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Datum nástupu do práce nemůže být v budoucnosti.");
        }

    }
}