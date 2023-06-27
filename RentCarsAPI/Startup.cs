using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RentCarsAPI.Entities;
using RentCarsAPI.Middleware;
using RentCarsAPI.Models.Car;
using RentCarsAPI.Models.Client;
using RentCarsAPI.Models.Hire;
using RentCarsAPI.Models.User;
using RentCarsAPI.Models.Validators;
using RentCarsAPI.Services;

namespace RentCarsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();

            services.AddDbContext<RentDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RentCarDbConnection")));

            services.AddScoped<RentSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHireService, HireService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IValidator<CreateCarDto>, CreateCarValidator>();
            services.AddScoped<IValidator<CreateClientDto>, CreateClientValidator>();
            services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
            services.AddScoped<IValidator<CreateHireDto>, CreateHireValidator>();
            services.AddScoped<IValidator<UpdateHireDto>, UpdateHireValidator>();
            services.AddScoped<IValidator<UpdateCarDto>, UpdateCarValidator>();
            services.AddScoped<IValidator<UpdateClientDto>, UpdateClientValidator>();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendClient", builder =>

                    builder.AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowAnyOrigin()
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RentSeeder seeder)
        {

            app.UseCors("FrontendClient");
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
            });

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
