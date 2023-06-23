using System.Collections.Generic;

namespace RentCarsAPI.Entities
{
    public class Car
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
        public bool EfficientNow { get; set; } //Czy jest sprawne
        public bool? AvailableNow { get; set; }
        public double PriceForDay { get; set; }
        public string Comments { get; set; }

        public virtual List<Hire> Hires{get; set ;}

    }
}
