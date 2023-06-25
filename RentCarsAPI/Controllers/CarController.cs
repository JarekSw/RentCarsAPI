using Microsoft.AspNetCore.Mvc;
using RentCarsAPI.Models.Car;
using RentCarsAPI.Services;
using System.Collections.Generic;

namespace RentCarsAPI.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCar([FromRoute] int id)
        {
            _carService.Delete(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateCarDto updateCarDto)
        {
            _carService.Update(id, updateCarDto);

            return Ok();
        }

        [HttpPost]
        public ActionResult CreateCar([FromBody] CreateCarDto dto)
        {
            var newCarId = _carService.Create(dto);

            return Created($"api/cars/{newCarId}", null);
        }

        [HttpGet("{id}")]
        public ActionResult<CarDto> GetById([FromRoute] int id)
        {
            var carDto = _carService.GetById(id);

            return Ok(carDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<CarDto>> GetAll()
        {
            var carDtos = _carService.GetAll();
            return Ok(carDtos);
        }

        //filt możliwy po dostępności, ilości miejsc i po marce
        [HttpGet]
        [Route("filtr")]
        public ActionResult<IEnumerable<CarDto>> GetBy([FromHeader] bool? isAvailable, [FromHeader] int? countPlace,
            [FromHeader] string? model, [FromHeader]string? mark)
        {
            var carDtos = _carService.GetBy(isAvailable, countPlace,model, mark);

            return Ok(carDtos);
        }
    }
}
