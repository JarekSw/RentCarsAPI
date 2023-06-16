using AutoMapper;
using RentCarsAPI.Entities;
using RentCarsAPI.Models.Car;
using RentCarsAPI.Models.Client;
using RentCarsAPI.Models.User;
using RentCarsAPI.Models.Hire;

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

            CreateMap<Hire, HireDto>()
                .ForMember(h => h.FirstName, x => x.MapFrom(m => m.Client.FirstName))
                .ForMember(h => h.LastName, x => x.MapFrom(m => m.Client.LastName))
                .ForMember(h => h.PESELOrPassportNumber, x => x.MapFrom(m => m.Client.PESELOrPassportNumber))
                .ForMember(h => h.CarMark, x => x.MapFrom(m => m.Car.Mark))
                .ForMember(h => h.CarModel, x => x.MapFrom(m => m.Car.Model))
                .ForMember(h => h.RegistrationNumber, x => x.MapFrom(m => m.Car.RegistrationNumber));

        }
    }
}
