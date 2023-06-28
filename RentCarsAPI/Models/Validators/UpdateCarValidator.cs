using FluentValidation;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Car;
using System.Linq;

namespace RentCarsAPI.Models.Validators
{
    public class UpdateCarValidator : AbstractValidator<UpdateCarDto>
    {
        public UpdateCarValidator(RentDbContext dbContext)
        {

            RuleFor(c => c.RegistrationNumber)
                .MinimumLength(5)
                .MaximumLength(7);



            RuleFor(c => c.VINNumer)
                .Length(17);
        }
    }
}
