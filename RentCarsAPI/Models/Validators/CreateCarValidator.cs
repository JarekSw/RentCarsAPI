using FluentValidation;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Car;
using System.Linq;

namespace RentCarsAPI.Models.Validators
{
    public class CreateCarValidator : AbstractValidator<CreateCarDto>
    {
        public CreateCarValidator(RentDbContext dbContext)
        {

            RuleFor(c => c.RegistrationNumber)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(7);

            RuleFor(c => c.RegistrationNumber)
                .Custom((value, context) =>
                {
                    var registrationNumberInUse = dbContext.Cars.Any(c => c.RegistrationNumber.Equals(value));
                    if (registrationNumberInUse)
                    {
                        context.AddFailure("RegistrationNumber", "That registrationNumber is taken");
                    }
                });

            RuleFor(c => c.Mark)
                .NotEmpty();

            RuleFor(c => c.Model)
                .NotEmpty();

            RuleFor(c => c.VINNumer)
                .NotEmpty()
                .Length(17);

            RuleFor(c => c.PriceForDay)
                .NotEmpty();

            RuleFor(c => c.CountPlace)
                .NotEmpty();
        }
    }
}
