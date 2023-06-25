using System;

namespace RentCarsAPI.Entities
{
    public class Hire
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public DateTime HireDate { get; set; } // data wyporzyczenia
        public DateTime ExpectedDateOfReturn { get; set; } //przewidywana data zwrotu 
        public DateTime? DateOfReturn { get; set; } //data zwrotu 
        public string? Comment { get; set; }
    }
}
