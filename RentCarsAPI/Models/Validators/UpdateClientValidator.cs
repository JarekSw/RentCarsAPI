using FluentValidation;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Client;
using System.Collections.Generic;
using System.Linq;

namespace RentCarsAPI.Models.Validators
{
    public class UpdateClientValidator : AbstractValidator<UpdateClientDto>
    {
        public UpdateClientValidator(RentDbContext dbContext)
        {
            RuleFor(c => c.PESELOrPassportNumber)
              .Length(9,11);
               

        }
    }
}
