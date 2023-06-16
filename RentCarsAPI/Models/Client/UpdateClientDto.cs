namespace RentCarsAPI.Models.Client
{
    public class UpdateClientDto
    {
        public string? LastName { get; set; }

        public string? email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DrivingLicenseCategory { get; set; }
        public bool? IsBlocked { get; set; }
        public string? Comments { get; set; }
    }
}
