using FluentValidation;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Hire;
using System.Linq;

namespace RentCarsAPI.Models.Validators
{
    public class UpdateHireValidator : AbstractValidator<UpdateHireDto>
    {

        public UpdateHireValidator(RentDbContext dbContext)
        {


            //RuleFor(h => h.ClientId)
            //    .Custom((value, context) =>
            //    {
            //        var client = dbContext.Clients.FirstOrDefault(c => c.Id == value);

            //        if ((client == null)&&client!=null)
            //            context.AddFailure("ClientId", "That client not exist or is blocked");
            //    });

            //RuleFor(h => h.CarId)
            //   .Custom((value, context) =>
            //   {
            //       var car = dbContext.Cars.FirstOrDefault(c => c.Id == value);

            //       if ((car == null || car.AvailableNow == false || car.EfficientNow == false)&&car!=null)
            //           context.AddFailure("CarId", "That car not exist or is not available");
            //   });

        }
    }
}
