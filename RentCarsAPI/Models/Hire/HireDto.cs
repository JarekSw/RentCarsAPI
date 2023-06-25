using System;

//refaktor
public enum HireStatus
{
    aktywny = 0,
    zakonczony = 1,
    opozniony = -1 
}
//


namespace RentCarsAPI.Models.Hire
{
    public class HireDto : IComparable
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarMark { get; set; }
        public string CarModel { get; set; }
        public string RegistrationNumber { get; set; }
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESELOrPassportNumber { get; set; }
        public DateTime HireDate { get; set; } // data wyporzyczenia
        public DateTime ExpectedDateOfReturn { get; set; } //przewidywana data zwrotu 
        public DateTime? DateOfReturn { get; set; } //data zwrotu 
        public string Comment { get; set; }
        
        //refaktor
        public HireStatus Status { get; set; }
        //
       public double Price { get; set; }
        public int CompareTo(object obj)
        {
            HireDto hireDto = obj as HireDto;

            if (hireDto.HireDate.CompareTo(HireDate) == 1)
                return 1;
            return -1;
        }
    }
}
