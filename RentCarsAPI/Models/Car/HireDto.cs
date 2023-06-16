using RentCarsAPI.Entities;
using System;

namespace RentCarsAPI.Models.Car
{
    public class HireDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? DateOfReturn { get; set; }
        public DateTime ExpectedDateOfReturn { get; set; }
        public bool Active { get; set; }

        public string Comment { get; set; }


        // info o aucie

        public string RegistrationNumber { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public double PriceForDay { get; set; }


        //

        //info o kliencie

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESELOrPassportNumber { get; set; }
        public string email { get; set; }
        public string PhoneNumber { get; set; }

        //



    }
}
