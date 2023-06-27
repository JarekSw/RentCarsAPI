using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentCarsAPI.Entities;
using RentCarsAPI.Exceptions;
using RentCarsAPI.Models.User;
using System.Linq;

namespace RentCarsAPI.Services
{
    public interface IUserService
    {
        int Create([FromBody] CreateUserDto userDto);
        UserDto GetById(int id);
        void Delete(int id);
        void Update(string emial, UpdateUserDto userDto);
    }
    public class UserService : IUserService
    {
        private readonly RentDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(RentDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void Update(string emial, UpdateUserDto userDto)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == emial);

            if (user is null)
                throw new NotFoundException("User not found");

            if(userDto!=null)
                user.HashPassword = userDto.HashPassword;
            if(user.Email!=null)
                user.Email = userDto.Email;

            _dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var users = _dbContext.Users.ToList();

            if (users.Count == 1)
                throw new NotFoundException("Only one user!");

            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            if (user is null)
                throw new NotFoundException("User not found");

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }
        public int Create(CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            _dbContext.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }
        public UserDto GetById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            if (user is null)
                throw new NotFoundException("User not found");

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}
