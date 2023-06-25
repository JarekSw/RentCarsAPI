using FluentValidation;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.User;
using System.Linq;

namespace RentCarsAPI.Models.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator(RentDbContext dbContext)
        {
            RuleFor(u => u.Email)
                .NotEmpty();

            RuleFor(u => u.HashPassword)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(c => c.Email)
              .Custom((value, context) =>
              {
                  var registrationNumberInUse = dbContext.Users.Any(c => c.Email.Equals(value));
                  if (registrationNumberInUse)
                  {
                      context.AddFailure("Email", "That email is taken");
                  }
              });
        }
    }
}
