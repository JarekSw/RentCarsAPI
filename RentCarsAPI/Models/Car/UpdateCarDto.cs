namespace RentCarsAPI.Models.Car
{
    public class UpdateCarDto
    {
        public string RegistrationNumber { get; set; }
        public bool? EfficientNow { get; set; } //Czy jest sprawne

        public bool? AvailableNow { get; set; } //Czy jest sprawne
        public double? PriceForDay { get; set; }
        public string Comments { get; set; }
    }
}
