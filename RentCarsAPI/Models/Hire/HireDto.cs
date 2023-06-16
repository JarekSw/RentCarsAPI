using System;
using RentCarsAPI.Entities;

namespace RentCarsAPI.Models.Hire
{
    public class HireDto
    {
        public int Id { get; set; }

        public string CarMark { get; set; }
        public string CarModel { get; set; }
        
        public string RegistrationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESELOrPassportNumber { get; set; }
        
        public DateTime HireDate { get; set; } // data wyporzyczenia
        public DateTime ExpectedDateOfReturn { get; set; } //przewidywana data zwrotu 
        public DateTime DateOfReturn { get; set; } //data zwrotu 

        public string Comment { get; set; }
    }
}
