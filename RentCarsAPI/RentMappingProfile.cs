using AutoMapper;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Car;
using RentCarsAPI.Models.Client;
using RentCarsAPI.Models.User;

namespace RentCarsAPI
{
    public class RentMappingProfile:Profile
    {

        public RentMappingProfile()
        {



            CreateMap<Car, CarDto>()
                .ForMember(c => c.AvailableNow, h => h.MapFrom(s =>s.EfficientNow));

            CreateMap<CreateCarDto, Car>();

            CreateMap<Client, ClientDto>();
            CreateMap<CreateClientDto, Client>();
            CreateMap<CreateUserDto,User>();
            CreateMap<User, UserDto>();
        }
    }
}
