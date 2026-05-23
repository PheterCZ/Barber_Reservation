using FluentValidation;
using backend.DTOs;

namespace backend.Validators
{
    public class AppointmentDtoValidator : AbstractValidator<AppointmentDto>
    {
        public AppointmentDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("ID zákazníka je povinné.");

            RuleFor(x => x.BarberId)
                .NotEmpty().WithMessage("ID barbera je povinné.");

            RuleFor(x => x.Service)
                .NotEmpty().WithMessage("Název služby je povinný.")
                .MaximumLength(100).WithMessage("Název služby může mít maximálně 100 znaků.");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Jméno zákazníka je povinné.");

            RuleFor(x => x.BarberName)
                .NotEmpty().WithMessage("Jméno barbera je povinné.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Čas začátku je povinný.")
                .Must(startTime => startTime > DateTime.Now).WithMessage("Čas začátku musí být v budoucnosti.");
                
            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("Čas konce je povinný.")
                .GreaterThan(x => x.StartTime).WithMessage("Čas konce musí být až po času začátku.");
        }
    }
}