﻿using AutoMapper;
using RentCarsAPI.Entities;
using RentCarsAPI.Exceptions;
using RentCarsAPI.Models.Client;
using System.Collections.Generic;
using System.Linq;

namespace RentCarsAPI.Services
{
    public interface IClientService
    {
        IEnumerable<ClientDto> GetAll();
        ClientDto GetById(int id);
        IEnumerable<ClientDto> GetByBlocked(bool blocked);

        int Create(CreateClientDto dto);
        void Update(int id, UpdateClientDto update);
        void Delete(int id);
    }

    public class ClientService : IClientService
    {
        private readonly RentDbContext _dbContext;
        private readonly IMapper _mapper;

        public ClientService(RentDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void Delete(int id)
        {
            var client = _dbContext.Clients.FirstOrDefault(x => x.Id == id);

            if (client is null)
                throw new NotFoundException("Client not found");

            if( _dbContext.Hires.FirstOrDefault(x => x.ClientId == id && x.DateOfReturn == null)!=null)
                throw new NotFoundException("Client have active hire");

            _dbContext.Clients.Remove(client);
            _dbContext.SaveChanges();
        }
        public void Update(int id, UpdateClientDto update)
        {
            var client = _dbContext.Clients.FirstOrDefault(x => x.Id == id);


        //    public string? FirstName { get; set; }
        //public string? LastName { get; set; }
        //public string? PESELOrPassportNumber { get; set; }
        //public string? email { get; set; }
        //public string? PhoneNumber { get; set; }
        //public string? DrivingLicenseCategory { get; set; }
        //public bool? IsBlocked { get; set; }
        //public string? Comments { get; set; }


            if (client is null)
                throw new NotFoundException("Client not found");

            if (update.FirstName != null)
                client.FirstName = update.FirstName;
            if (update.LastName != null)
                client.LastName = update.LastName;
            if (update.PESELOrPassportNumber != null)
            {
                if(_dbContext.Clients.FirstOrDefault(c=>c.PESELOrPassportNumber==(string)update.PESELOrPassportNumber&&client.PESELOrPassportNumber!=update.PESELOrPassportNumber )!=null)
                    throw new NotFoundException("PESEL or pasport number is taken");
                client.PESELOrPassportNumber = update.PESELOrPassportNumber;
            }
                
            if (update.email != null)
                client.email = update.email;
            if (update.PhoneNumber != null)
                client.PhoneNumber = update.PhoneNumber;
            if (update.DrivingLicenseCategory != null)
                client.DrivingLicenseCategory = update.DrivingLicenseCategory;
            if (update.IsBlocked != null)
                client.IsBlocked = (bool)update.IsBlocked;
            if (update.Comments != null)
                client.Comments = update.Comments;

            _dbContext.SaveChanges();
        }
        public int Create(CreateClientDto dto)
        {
            var clientEntity = _mapper.Map<Client>(dto);

            _dbContext.Clients.Add(clientEntity);
            _dbContext.SaveChanges();

            return clientEntity.Id;
        }
        public IEnumerable<ClientDto> GetByBlocked(bool blocked)
        {
            List<ClientDto> clientDtos = (List<ClientDto>)GetAll();
            List<ClientDto> clientWithFiltr = new List<ClientDto>();

            foreach (var dto in clientDtos)
            {
                if (dto.IsBlocked == blocked)
                {
                    clientWithFiltr.Add(dto);
                }
            }

            if (clientWithFiltr is null)
            {
                throw new NotFoundException("Clients not found");
            }

            clientWithFiltr.Sort();

            return clientWithFiltr;
        }
        public ClientDto GetById(int id)
        {
            var client = _dbContext.Clients.FirstOrDefault(cl => cl.Id == id);

            if (client is null)
            {
                throw new NotFoundException("Client not found");
            }

            var clientDto = _mapper.Map<ClientDto>(client);
            return clientDto;
        }
        public IEnumerable<ClientDto> GetAll()
        {
            var clients = _dbContext.Clients.ToList();

            if (clients is null)
            {
                throw new NotFoundException("Clients not found");
            }

            var clientsDto = _mapper.Map<List<ClientDto>>(clients);

            clientsDto.Sort();

            return clientsDto;
        }
    }
}
