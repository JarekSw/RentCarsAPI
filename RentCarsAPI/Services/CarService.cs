using AutoMapper;
using RentCarsAPI.Entities;
using RentCarsAPI.Exceptions;
using RentCarsAPI.Models.Car;
using System.Collections.Generic;
using System.Linq;

namespace RentCarsAPI.Services
{
    public interface ICarService
    {
        IEnumerable<CarDto> GetAll();
        IEnumerable<CarDto> GetBy(bool? isAvailable, int? countPlace, string? model, string? mark);

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
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == id);

            if (car is null)
                throw new NotFoundException("Cars not found");


            if (_dbContext.Hires.FirstOrDefault(h => h.CarId == id && h.DateOfReturn == null) != null) 
                throw new NotFoundException("Cars is on active hire");


            _dbContext.Remove(car);
            _dbContext.SaveChanges();
        }
        public void Update(int id, UpdateCarDto dto)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == id);

            //    public string? RegistrationNumber { get; set; }
            //public string? VINNumer { get; set; }
            //public string? Mark { get; set; }
            //public string? Model { get; set; }
            //public bool? AutomaticTransmission { get; set; }
            //public double? Horsepower { get; set; }
            //public int? CountPlace { get; set; }
            //public string? Category { get; set; }
            //public bool? EfficientNow { get; set; } //Czy jest sprawne
            //public bool? AvailableNow { get; set; }
            //public double? PriceForDay { get; set; }
            //public string? Comments { get; set; }


            if (car is null)
                throw new NotFoundException("Cars not found");

            if(dto.VINNumer!=null)
                car.VINNumer = (string) dto.VINNumer;
            if (dto.Mark != null)
                car.Mark = (string)dto.Mark;
            if (dto.Model != null)
                car.Model = (string)dto.Model;
            if (dto.AutomaticTransmission != null)
                car.AutomaticTransmission = (bool)dto.AutomaticTransmission;
            if (dto.Horsepower != null)
                car.Horsepower = (double)dto.Horsepower;
            if (dto.CountPlace != null)
                car.CountPlace = (int)dto.CountPlace;
            if (dto.Category != null)
                car.Category = (string)dto.Category;
            if (dto.PriceForDay != null)
                car.PriceForDay = (double)dto.PriceForDay;
            if (dto.RegistrationNumber != null)
            {
                

                if(_dbContext.Cars.FirstOrDefault(c=>c.RegistrationNumber==dto.RegistrationNumber)!=null&&
                    car.RegistrationNumber!=dto.RegistrationNumber)
                    throw new NotFoundException("Registration number is taken");
                car.RegistrationNumber = (string)dto.RegistrationNumber;
            }
                
            if (dto.EfficientNow != null)
                car.EfficientNow = (bool)dto.EfficientNow;
            if (dto.AvailableNow != null)
                car.AvailableNow = (bool)dto.AvailableNow;
            if (dto.Comments != null)
                car.Comments = (string)dto.Comments;

            _dbContext.SaveChanges();
        }
        public int Create(CreateCarDto dto)
        {
            var carEntities = _mapper.Map<Car>(dto);

            var newCarId = carEntities.Id;

            _dbContext.Cars.Add(carEntities);
            _dbContext.SaveChanges();

            return newCarId;
        }
        public CarDto GetById(int id)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == id);
            var carDto = _mapper.Map<CarDto>(car);

            if (carDto is null)
                throw new NotFoundException("Cars not found");

            return carDto;
        }
        public IEnumerable<CarDto> GetBy(bool? isAvailable, int? countPlace, string? model, string? mark)
        {
            var cars = GetAll();
            var carsDtos = _mapper.Map<List<CarDto>>(cars);

            if (carsDtos is null)
                throw new NotFoundException("Cars not exist");

            if (isAvailable != null)
            {
                carsDtos = (List<CarDto>)GetByAvailable((bool)isAvailable, carsDtos);
            }
            if (countPlace != null)
            {
                carsDtos = (List<CarDto>)GetByCountPlace((int)countPlace, carsDtos);
            }
            if (mark != null)
            {
                carsDtos = (List<CarDto>)GetByMark((string)mark, carsDtos);
            }
            if (model != null)
            {
                carsDtos = (List<CarDto>)GetByModel((string)model, carsDtos);
            }

            if (carsDtos is null)
                throw new NotFoundException("Cars not found");

            carsDtos.Sort();

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
        public IEnumerable<CarDto> GetByMark(string mark, List<CarDto> carDtos)
        {
            List<CarDto> cars = new List<CarDto>();

            foreach (CarDto item in carDtos)
            {
                if (item.Mark.ToLower() == mark.ToLower())
                    cars.Add(item);
            }
            return cars;
        }
        public IEnumerable<CarDto> GetByCountPlace(int countPlace, List<CarDto> carDtos)
        {
            var carsSorted = FiltrCarsByCountPlace(carDtos, countPlace);

            if (carsSorted.Count() == 0)
                throw new NotFoundException("Cars not found");

            return carsSorted;
        }
        public IEnumerable<CarDto> GetByAvailable(bool isAvailable, List<CarDto> carDtos)
        {
            var carsSorted = FiltrCarsByAvailable(carDtos, isAvailable);

            if (carsSorted.Count() == 0)
                throw new NotFoundException("Cars not found");

            return carsSorted;
        }
        public IEnumerable<CarDto> GetAll()
        {
            var cars = _dbContext.Cars.ToList();

            if (cars is null)
                throw new NotFoundException("Cars not found");

            var carsDtos = _mapper.Map<List<CarDto>>(cars);

            carsDtos.Sort();

            return carsDtos;
        }
        private IEnumerable<CarDto> FiltrCarsByAvailable(IEnumerable<CarDto> cars, bool isAvailable)
        {
            List<CarDto> carsDtos = new List<CarDto>();

            foreach (CarDto car in cars)
            {
                if (car.AvailableNow == isAvailable)
                    carsDtos.Add(car);
            }

            return carsDtos;
        }
        private IEnumerable<CarDto> FiltrCarsByCountPlace(IEnumerable<CarDto> cars, int countPlace)
        {
            List<CarDto> carsDtos = new List<CarDto>();

            foreach (CarDto car in cars)
            {
                if (car.CountPlace == countPlace)
                    carsDtos.Add(car);
            }

            return carsDtos;
        }
    }
}
