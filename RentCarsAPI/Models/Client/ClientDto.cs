using System;

namespace RentCarsAPI.Models.Client
{
    public class ClientDto : IComparable
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

        public int CompareTo(object obj)
        {
            ClientDto clientDto = obj as ClientDto;

            if (clientDto.LastName.CompareTo(LastName) == 1)
                return -1;
            else
                return 1;
        }
    }
}
