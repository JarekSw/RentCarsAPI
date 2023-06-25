using System;
using System.ComponentModel.DataAnnotations;

namespace RentCarsAPI.Models.Car
{
    public class CarDto : IComparable
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string VINNumer { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public bool AutomaticTransmission { get; set; }
        public double Horsepower { get; set; }
        public int CountPlace { get; set; }
        public string Category { get; set; }
        public bool EfficientNow { get; set; }
      
        public bool AvailableNow { get; set; }
        public double PriceForDay { get; set; }
        public string Comments { get; set; }

        public int CompareTo(object obj)
        {
            CarDto carDto = obj as CarDto;

            if (carDto.RegistrationNumber.CompareTo(RegistrationNumber) == 1)
                return -1;
            return 1;
        }
    }
}
