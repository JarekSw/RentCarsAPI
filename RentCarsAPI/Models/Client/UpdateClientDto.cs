using System.ComponentModel.DataAnnotations;

namespace RentCarsAPI.Models.Client
{
    public class UpdateClientDto
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [RegularExpression(@"^\d{11}$|^[A-Za-z]{2}\d{7}$")]
        public string? PESELOrPassportNumber { get; set; }
        [EmailAddress]
        public string? email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [RegularExpression(@"^[ABCD]$")]
        public string? DrivingLicenseCategory { get; set; }
        public bool? IsBlocked { get; set; }
        public string? Comments { get; set; }
    }
}
