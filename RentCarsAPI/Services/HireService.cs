using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCarsAPI.Entities;
using RentCarsAPI.Exceptions;
using RentCarsAPI.Models.Hire;
using System.Collections.Generic;
using System.Linq;

namespace RentCarsAPI.Services
{
    public interface IHireService
    {
        IEnumerable<HireDto> GetAll();
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

        public IEnumerable<HireDto> GetAll()
        {
            var hires = _dbContext.Hires
                .Include(h=>h.Car)
                .Include(h=>h.Client)
                .ToList();

            if (hires is null)
                throw new NotFoundException("Hires not found");



            var hireDtos = _mapper.Map<List<HireDto>>(hires);

            return hireDtos;
        }
    }
}
