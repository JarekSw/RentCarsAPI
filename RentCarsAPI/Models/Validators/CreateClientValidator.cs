using FluentValidation;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Client;
using System.Linq;

namespace RentCarsAPI.Models.Validators
{
    public class CreateClientValidator: AbstractValidator<CreateClientDto>
    {
        public CreateClientValidator(RentDbContext dbContext)
        {
            RuleFor(c => c.FirstName)
             .NotEmpty();
            RuleFor(c => c.LastName)
                .NotEmpty();
            RuleFor(c => c.PESELOrPassportNumber)
                .NotEmpty();
            RuleFor(c => c.PhoneNumber)
                .NotEmpty();
            RuleFor(c => c.email)
                .NotEmpty();

            RuleFor(c => c.PESELOrPassportNumber)
              .Custom((value, context) =>
              {
                  var registrationNumberInUse = dbContext.Clients.Any(c => c.PESELOrPassportNumber.Equals(value));
                  if (registrationNumberInUse)
                  {
                      context.AddFailure("PESELOrPassportNumber", "That PESELOrPassportNumber is taken");
                  }
              });
        }
    }
}
