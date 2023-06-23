using System;

namespace RentCarsAPI.Models.Hire
{
    public class UpdateHireDto
    {
        public DateTime? DateOfReturn { get; set; }

        public string? Comment { get; set;}
    }
}
