using System.Collections.Generic;

namespace RentCarsAPI.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESELOrPassportNumber { get; set; }
        public string email { get; set; }
        public string PhoneNumber { get; set; }
        public string DrivingLicenseCategory { get; set; }
        public bool IsBlocked { get; set; }
        public string Comments { get; set; }

        public virtual List<Hire> Hires { get; set; }

    }
}
