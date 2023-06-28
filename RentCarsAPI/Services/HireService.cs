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
        List<HireDto> GetByFiltr( int? clientId, int? carId, HireStatus? hireStatus);
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
        public List<HireDto> GetByFiltr(int? clientId, int? carId, HireStatus? hireStatus)
        {
            var hires = _dbContext.Hires
                .Include(h => h.Car)
                .Include(h => h.Client)
                .ToList();

            if (hires is null)
                throw new NotFoundException("Hires not exist");


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
      
        public double Finish(int id, FinishHireDto date)
        {
            var hire = _dbContext.Hires.FirstOrDefault(h => h.Id == id);
            hire.DateOfReturn = date.DateOfReturn;
            
            _dbContext.Cars.FirstOrDefault(c => c.Id == hire.CarId).AvailableNow = true;
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
            hireEntities.Car = new Car();
            hireEntities.Car = _dbContext.Cars.FirstOrDefault(c => c.Id == hireEntities.CarId);
            hireEntities.Client = new Client();
            hireEntities.Client = _dbContext.Clients.FirstOrDefault(c => c.Id == hireEntities.ClientId);

            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == dto.CarId);
            car.AvailableNow = false;

            _dbContext.Hires.Add(hireEntities);
            _dbContext.SaveChanges();

            return hireEntities.Id;
        }
        public void Update(int HireId, UpdateHireDto dto)
        {
            var hire = _dbContext.Hires
                .Include(h=>h.Car)
                .Include(h=>h.Client)
                .FirstOrDefault(h => h.Id == HireId);

            if (hire is null)
            {
                throw new NotFoundException("Hire not found");
            }



            //    public int? CarId { get; set; }
            //public int? ClientId { get; set; }
            //public DateTime? HireDate { get; set; } // data wyporzyczenia
            //public DateTime? ExpectedDateOfReturn { get; set; } //przewidywana data zwrotu 
            //public DateTime? DateOfReturn { get; set; } //data zwrotu 
            //public string? Comment { get; set; }

            if(dto.HireDate!=null)
            {
                if(dto.HireDate>hire.DateOfReturn||dto.HireDate>hire.ExpectedDateOfReturn)
                    throw new NotFoundException("Bad date, date of return or expected return is earlier than this date!");
                hire.HireDate = (DateTime)dto.HireDate;
            }
            if (dto.ExpectedDateOfReturn != null)
            {
                if (dto.ExpectedDateOfReturn < hire.HireDate)
                    throw new NotFoundException("Bad date, date is earlier than date of hire!");
                hire.ExpectedDateOfReturn = (DateTime)dto.ExpectedDateOfReturn;
            }
            if (dto.DateOfReturn != null)
            {
                if (dto.DateOfReturn < hire.HireDate)
                    throw new NotFoundException("Bad date, date is earlier than date of hire!");
                hire.DateOfReturn = dto.DateOfReturn;
            }
            if (dto.ClientId!= null)
            {
                if(_dbContext.Clients.FirstOrDefault(x=>x.Id==dto.ClientId)==null)
                    throw new NotFoundException("Client not exist!");

                hire.ClientId=(int)dto.ClientId;
                hire.Client = _dbContext.Clients.FirstOrDefault(c => c.Id == hire.ClientId);
            }

            if (dto.CarId != null)
            {
                var car = _dbContext.Cars.FirstOrDefault(c => c.Id == dto.CarId);
                if (car is null)
                    throw new NotFoundException("Car not exist!");

                if (_mapper.Map<HireDto>(hire).Status != HireStatus.zakonczony)
                {
                    
                    
                    if(car.AvailableNow==false)
                        throw new NotFoundException("Hire can not edit, car is not available!");

                    _dbContext.Cars.FirstOrDefault(c => c.Id == hire.CarId).AvailableNow = true;
                    _dbContext.Cars.FirstOrDefault(c=>c.Id==dto.CarId).AvailableNow = false;
                }
               
                hire.CarId=(int)dto.CarId; 
            
                hire.Car = car;
            }

            

            if (dto.Comment != null)
                hire.Comment = dto.Comment;

            if (hire.DateOfReturn < hire.HireDate || hire.ExpectedDateOfReturn < hire.HireDate)
                throw new NotFoundException("Wrong the one of date!");

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
            DateTime d= DateTime.Today;

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
