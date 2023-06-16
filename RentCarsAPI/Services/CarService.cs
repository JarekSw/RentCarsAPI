using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentCarsAPI.Entities;
using RentCarsAPI.Exceptions;
using RentCarsAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentCarsAPI.Services
{
    public interface ICarService
    {
        IEnumerable<CarDto> GetAll();
        IEnumerable<CarDto> GetBy(bool? isAvailable, int? countPlace, string? model);

        CarDto GetById(int id);
        int Create(CreateCarDto dto);
        void Update(int id, UpdateCarDto dto);
        void Delete(int id);
    }
    
    public class CarService : ICarService
    {
        private readonly IMapper _mapper;
        private readonly RentDbContext _dbContext;

        public CarService(RentDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }


        public void Delete(int id)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id== id);

            if (car is null)
                throw new NotFoundException("Cars not found");

            _dbContext.Remove(car);
            _dbContext.SaveChanges();
        }

        public void Update(int id, UpdateCarDto dto)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == id);

            if ( car is null)
                throw new NotFoundException("Cars not found");
        

            if (dto.PriceForDay != null)
                car.PriceForDay = (double)dto.PriceForDay; 
            if (dto.RegistrationNumber != null)
                car.RegistrationNumber = (string)dto.RegistrationNumber;
            if (dto.EfficientNow != null)
                car.EfficientNow = (bool)dto.EfficientNow;
            if (dto.Comments != null)
                car.Comments = (string)dto.Comments;

            _dbContext.SaveChanges();

        }
        public int Create(CreateCarDto dto)
        {
            var carEntities= _mapper.Map<Car>(dto);

            var newCarId = carEntities.Id;

            _dbContext.Cars.Add(carEntities);
            _dbContext.SaveChanges();

            return newCarId;
        }
        public CarDto GetById(int id)
        {
            var car=_dbContext.Cars.FirstOrDefault(c=>c.Id==id);
            var carDto=_mapper.Map<CarDto>(car);

            if (carDto is null)
                throw new NotFoundException("Cars not found");

            return carDto;
        }

        public IEnumerable<CarDto> GetBy(bool? isAvailable, int? countPlace, string? model)
        {
            var cars=GetAll();
            var carsDtos=_mapper.Map<List<CarDto>>(cars); 

            if(isAvailable != null) 
            {
                carsDtos =(List<CarDto>) GetByAvailable((bool)isAvailable,carsDtos);
            }
            if (countPlace != null)
            {
                carsDtos = (List<CarDto>)GetByCountPlace((int)countPlace, carsDtos);
            } 
            if (model != null)
            {
                carsDtos = (List<CarDto>)GetByModel((string)model, carsDtos);
            }

            if(carsDtos is null)
                throw new NotFoundException("Cars not found");
            return carsDtos;
        }

        public IEnumerable<CarDto> GetByModel(string model, List<CarDto> carDtos)
        {
            List<CarDto> cars = new List<CarDto>();

            foreach (CarDto item in carDtos)
            {
                if (item.Model.ToLower() == model.ToLower()) 
                    cars.Add(item);
            }
            return cars;
        }
        public IEnumerable<CarDto> GetByCountPlace( int countPlace, List<CarDto> carDtos)
        {      

            var carsSorted = FiltrCarsByCountPlace(carDtos, countPlace);

            if (carsSorted.Count() == 0)
                throw new NotFoundException("Cars not found");

            return carsSorted;

        }

        public IEnumerable<CarDto> GetByAvailable(bool isAvailable, List<CarDto> carDtos)
        {
           
            
            var carsSorted=FiltrCarsByAvailable(carDtos, isAvailable);

            if (carsSorted.Count()==0)
                throw new NotFoundException("Cars not found");

            return carsSorted;


        }
        public IEnumerable<CarDto> GetAll()
        {
            var cars = _dbContext.Cars.ToList();

            if (cars is null)
                throw new NotFoundException("Cars not found");

            var carsDtos = _mapper.Map<List<CarDto>>(cars);

            


            return carsDtos;
        }
        private IEnumerable<CarDto> FiltrCarsByAvailable(IEnumerable<CarDto> cars , bool isAvailable)
        {
            List<CarDto> carsDtos= new List<CarDto>();

            foreach (CarDto car in cars)
            {
                if(car.AvailableNow==isAvailable)
                    carsDtos.Add(car);
            }

            return carsDtos;
        }
        private IEnumerable<CarDto> FiltrCarsByCountPlace(IEnumerable<CarDto> cars , int countPlace)
        {
            List<CarDto> carsDtos= new List<CarDto>();

            foreach (CarDto car in cars)
            {
                if(car.CountPlace==countPlace)
                    carsDtos.Add(car);
            }

            return carsDtos;
        }
    }
}
