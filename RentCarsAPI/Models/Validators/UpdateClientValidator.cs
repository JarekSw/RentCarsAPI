using FluentValidation;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Client;
using System.Linq;

namespace RentCarsAPI.Models.Validators
{
    public class UpdateClientValidator : AbstractValidator<UpdateClientDto>
    {
        public UpdateClientValidator(RentDbContext dbContext)
        {
            RuleFor(c => c.PESELOrPassportNumber)
              .Custom((value, context) =>
              {
                  var registrationNumberInUse = dbContext.Clients.Any(c => c.PESELOrPassportNumber.Equals(value));
                  if (registrationNumberInUse)
                  {
                      context.AddFailure("PESELOrPassportNumber", "That PESELOrPassportNumber is taken");
                  }
              })
              .Length(9,11);
               

        }
    }
}
