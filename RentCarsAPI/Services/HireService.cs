using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCarsAPI.Entities;
using RentCarsAPI.Exceptions;
using RentCarsAPI.Models.Hire;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentCarsAPI.Services
{
    public interface IHireService
    {
        IEnumerable<HireDto> GetAll();
        void Update(int HireId, UpdateHireDto dto);
        HireDto GetById(int id);
        int Create(CreateHireDto dto);
        double Finish(int id, FinishHireDto dateOfReturn);
        List<HireDto> GetByFiltr(bool? isFinished, int? clientId, int? carId, HireStatus? hireStatus);
        void Delete(int id);
    }
    public class HireService : IHireService
    {
        private readonly RentDbContext _dbContext;
        private readonly IMapper _mapper;

        public HireService(RentDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void Delete(int id)
        {
            var hire = _dbContext.Hires.FirstOrDefault(h => h.Id == id);

            if (hire is null)
                throw new NotFoundException("Hire not found");

            _dbContext.Hires.Remove(hire);
            _dbContext.SaveChanges();
        }
        public List<HireDto> GetByFiltr(bool? isFinished, int? clientId, int? carId, HireStatus? hireStatus)
        {
            var hires = _dbContext.Hires
                .Include(h => h.Car)
                .Include(h => h.Client)
                .ToList();

            if (hires is null)
                throw new NotFoundException("Hires not exist");

            if (isFinished != null)
            {
                hires = GetByIsFinished((bool)isFinished, hires);
            }
            if (clientId != null)
            {
                hires = GetByClientId((int)clientId, hires);
            }
            if (carId != null)
            {
                hires = GetBycarId((int)carId, hires);
            }
            if (hireStatus != null)
            {
                hires = GetByHireStatus((HireStatus)carId, hires);
            }
            if (hires.Count == 0)
                throw new NotFoundException("Hires with this filtr not found");

            var hireDtos = _mapper.Map<List<HireDto>>(hires);

            hireDtos.Sort();

            return hireDtos;
        }
        public List<Hire> GetByHireStatus(HireStatus hireStatus, List<Hire> hires)
        {
            List<Hire> result = new List<Hire>();

            foreach (var hire in hires)
            {
                var hireDto=_mapper.Map<HireDto>(hire);
                if (hireDto.Status==hireStatus)
                    result.Add(hire);
            }
            return result;
        }
        public List<Hire> GetBycarId(int carId, List<Hire> hires)
        {
            List<Hire> result = new List<Hire>();

            foreach (var hire in hires)
            {
                if (hire.CarId == carId)
                    result.Add(hire);
            }
            return result;
        }
        public List<Hire> GetByClientId(int clientId, List<Hire> hires)
        {
            List<Hire> result = new List<Hire>();

            foreach (var hire in hires)
            {
                if (hire.ClientId == clientId)
                    result.Add(hire);
            }
            return result;
        }
        public List<Hire> GetByIsFinished(bool isFinished, List<Hire> hires)
        {
            List<Hire> result = new List<Hire>();

            foreach (var hire in hires)
            {
                if (hire.DateOfReturn == null)
                {
                    if (isFinished == false)
                        result.Add(hire);
                }
                else
                {
                    if (isFinished == true)
                        result.Add(hire);
                }
            }
            return result;
        }
        public double Finish(int id, FinishHireDto date)
        {
            var hire = _dbContext.Hires.FirstOrDefault(h => h.Id == id);
            hire.DateOfReturn = date.DateOfReturn;
            
            _dbContext.Cars.FirstOrDefault(c => c.Id == hire.CarId).EfficientNow = true;
            _dbContext.SaveChanges();

            return CalculatePrice(hire);
        }
        public int Create(CreateHireDto dto)
        {
            if (dto.HireDate > dto.ExpectedDateOfReturn)
                throw new NotFoundException("Bad hire or expected date of return");
            if (dto.HireDate > dto.DateOfReturn)
                throw new NotFoundException("Bad hire or date of return");

            var hireEntities = _mapper.Map<Hire>(dto);

            hireEntities.Car = _dbContext.Cars.FirstOrDefault(c => c.Id == hireEntities.CarId);
            hireEntities.Client = _dbContext.Clients.FirstOrDefault(c => c.Id == hireEntities.ClientId);

            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == dto.CarId);
            car.AvailableNow = false;

            _dbContext.Hires.Add(hireEntities);
            _dbContext.SaveChanges();

            return hireEntities.Id;
        }
        public void Update(int HireId, UpdateHireDto dto)
        {
            var hire = _dbContext.Hires.FirstOrDefault(h => h.Id == HireId);

            if (hire is null)
            {
                throw new NotFoundException("Hire not found");
            }

            if (dto.DateOfReturn != null)
            {
                hire.DateOfReturn = (System.DateTime)dto.DateOfReturn;
            }

            if (dto.Comment != null)
                hire.Comment = dto.Comment;

            _dbContext.SaveChanges();
        }
        public HireDto GetById(int id)
        {
            var hire = _dbContext.Hires
                .Include(h => h.Car)
                .Include(h => h.Client)
                .FirstOrDefault(h => h.Id == id);

            if (hire is null)
                throw new NotFoundException("Hire not found");

            var hireDto = _mapper.Map<HireDto>(hire);

            return hireDto;
        }
        public IEnumerable<HireDto> GetAll()
        {
            var hires = _dbContext.Hires
                .Include(h => h.Car)
                .Include(h => h.Client)
                .ToList();

            if (hires is null)
                throw new NotFoundException("Hires not found");

            var hireDtos = _mapper.Map<List<HireDto>>(hires);

            hireDtos.Sort();

            return hireDtos;
        }

        public static double CalculatePrice(Hire dto)
        {
            double days = (dto.ExpectedDateOfReturn - dto.HireDate).TotalDays;
            double price = days*dto.Car.PriceForDay;

            if(dto.DateOfReturn!=null)
            {
                if (dto.ExpectedDateOfReturn < dto.DateOfReturn)
                    price += ((DateTime)dto.DateOfReturn - dto.ExpectedDateOfReturn).TotalDays * dto.Car.PriceForDay*2.5;
                if (dto.ExpectedDateOfReturn > dto.DateOfReturn)
                    price -= (dto.ExpectedDateOfReturn - (DateTime)dto.DateOfReturn).TotalDays * dto.Car.PriceForDay * 0.5;
            }    

            return price;
        }
    }
}
