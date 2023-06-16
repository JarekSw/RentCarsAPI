﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentCarsAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RentCarsAPI
{
    public class RentSeeder
    {
        private readonly RentDbContext _dbContext;

        public RentSeeder(RentDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {

                var pandingMigrations= _dbContext.Database.GetPendingMigrations();

                if(pandingMigrations != null && pandingMigrations.Any()) 
                {
                    _dbContext.Database.Migrate();
                }

               if(!_dbContext.Clients.Any())
                {
                    var roles = GetClients();
                    _dbContext.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if(!_dbContext.Cars.Any())
                {
                    var roles = GetCars();
                    _dbContext.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if(!_dbContext.Hires.Any())
                {
                    var roles = GetHires();
                    _dbContext.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if(!_dbContext.Users.Any())
                {
                    var roles = GetUser();
                    _dbContext.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }
        private User GetUser()
        {
            var result = new User()
            {
                Email = "email.email.com",
                HashPassword = "password"
            };

            return result;
        }
        private IEnumerable<Car> GetCars()
        {
            var result = new List<Car>()
            {             
                new Car()
                {
                    RegistrationNumber="Test324",
                    VINNumer="Test75",
                    Mark="TestMark3",
                    Model="TestMode34",
                    Horsepower=200,
                    CountPlace=2,
                    Category="B",
                    EfficientNow=false,
                    Comments="samochod sportowy"

                }
            };
            return result;
        }

        public IEnumerable<Client> GetClients()
        {
            var result = new List<Client>()
            {
                
                new Client()
                {
                    FirstName="Daniel",
                    LastName="Mroczek",
                    PESELOrPassportNumber="112",
                    email="opierdalam@sie_i_nic_nie.robie",
                    IsBlocked=true,
                    PhoneNumber="997",
                    DrivingLicenseCategory="B",
                },

            };
            return result;
        }
        DateTime thisDate1 = new DateTime(2023, 5, 12);
        DateTime thisDate2 = new DateTime(2023, 5, 30);
        private IEnumerable<Hire> GetHires()
        {
            var result = new List<Hire>()
            {
                new Hire()
                {
                    Car=new Car()
                    {
                        RegistrationNumber="Test623",
                        VINNumer="Test442",
                        Mark="TestMark2",
                        Model="TestModel2",
                        Horsepower=90,
                        CountPlace=4,
                        Category="B",
                        EfficientNow=true,
                        Comments="uszkodzona kierwnica"

                    },
                    Client=new Client()
                    {
                        FirstName="Mateusz",
                        LastName="Leszczuk",
                        PESELOrPassportNumber="112",
                        email="gruzlik@choroba.zakazna",
                        IsBlocked=false,
                        PhoneNumber="997",
                        DrivingLicenseCategory=("B")
                    },
                    HireDate=DateTime.Today,
                },
                new Hire()
                {
                    Car= new Car()
                    {
                        RegistrationNumber="Test123",
                        VINNumer="Test432",
                        Mark="TestMark1",
                        Model="TestModel1",
                        AutomaticTransmission=false,
                        Horsepower=86,
                        CountPlace=5,
                        Category="B",
                        EfficientNow=true
                    },
                    Client=new Client()
                    {
                        FirstName="Adam",
                        LastName="Cieślak",
                        PESELOrPassportNumber="112",
                        email="adam@adam.adam",
                        PhoneNumber="997",
                        Comments="Frontend developer",
                        DrivingLicenseCategory=("B")
                    },

                    HireDate = thisDate1,
                    DateOfReturn=thisDate2
                }
            };
            return result;
        }
    }
}
