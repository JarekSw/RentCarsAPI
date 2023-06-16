using AutoMapper;
using RentCarsAPI.Entities;
using RentCarsAPI.Models;

namespace RentCarsAPI
{
    public class RestaurantMappingProfile:Profile
    {

        public RestaurantMappingProfile()
        {



            CreateMap<Car, CarDto>()
                .ForMember(c => c.AvailableNow, h => h.MapFrom(s =>s.EfficientNow));

            CreateMap<CreateCarDto, Car>();
        }
    }
}
