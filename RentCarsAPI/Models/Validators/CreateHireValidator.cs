using FluentValidation;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Client;
using RentCarsAPI.Models.Hire;
using System.Data;
using System.Linq;

namespace RentCarsAPI.Models.Validators
{
    public class CreateHireValidator: AbstractValidator<CreateHireDto>
    {

        public CreateHireValidator(RentDbContext dbContext)
        {
            RuleFor(h => h.ClientId).NotEmpty();
            RuleFor(h => h.ClientId)
                .Custom((value, context) =>
                {
                    var client = dbContext.Clients.FirstOrDefault(c => c.Id == value);

                    if (client == null || client.IsBlocked)
                        context.AddFailure("ClientId", "That client not exist or is blocked");
                });

            RuleFor(h => h.CarId).NotEmpty();
            RuleFor(h => h.CarId)
               .Custom((value, context) =>
               {
                   var car = dbContext.Cars.FirstOrDefault(c => c.Id == value);

                   if (car == null||car.AvailableNow==false||car.EfficientNow==false)
                       context.AddFailure("CarId", "That car not exist or is not available");
               });
            RuleFor(h=>h.ExpectedDateOfReturn).NotEmpty();
            
            RuleFor(h=>h.HireDate).NotEmpty();

        }
    }
}
