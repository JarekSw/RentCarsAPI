using System;

namespace RentCarsAPI.Models.Hire
{
    public class CreateHireDto
    {
        public int CarId { get; set; }
        public int ClientId { get; set; }
        public DateTime HireDate { get; set; } // data wyporzyczenia
        public DateTime ExpectedDateOfReturn { get; set; } //przewidywana data zwrotu 
        public DateTime? DateOfReturn { get; set; } //data zwrotu 

        public string? Comment { get; set; }
    }
}
