using System.ComponentModel.DataAnnotations;

namespace RentCarsAPI.Models.Client
{
    public class UpdateClientDto
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PESELOrPassportNumber { get; set; }
        [EmailAddress]
        public string? email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? DrivingLicenseCategory { get; set; }
        public bool? IsBlocked { get; set; }
        public string? Comments { get; set; }
    }
}
