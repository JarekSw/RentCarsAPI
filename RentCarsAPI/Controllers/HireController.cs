using Microsoft.AspNetCore.Mvc;
using RentCarsAPI.Models.Hire;
using RentCarsAPI.Services;
using System.Collections.Generic;

namespace RentCarsAPI.Controllers
{
    [ApiController]
    [Route("api/hires")]
    public class HireController : ControllerBase
    {
        private readonly IHireService _hireService;

        public HireController(IHireService service)
        {
            _hireService = service;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _hireService.Delete(id);

            return NoContent();
        }

        [HttpGet("filtr")]
        public ActionResult<IEnumerable<HireDto>> GetByFiltr([FromHeader] bool? isFinished, [FromHeader] int? clientId, [FromHeader] int? carId, [FromHeader] HireStatus? hireStatus )
        {
            var hireDtos = _hireService.GetByFiltr(isFinished, clientId, carId, hireStatus);

            return Ok(hireDtos);
        }

        [HttpGet("finish/{id}")]
        public ActionResult<double> Finish([FromRoute] int id, [FromBody] FinishHireDto dateOfReturn)
        {
            double price = _hireService.Finish(id, dateOfReturn);

            return Ok(price);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateHireDto createHireDto)
        {
            int newId = _hireService.Create(createHireDto);

            return Created($"api/hires/{newId}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateHireDto update)
        {
            _hireService.Update(id, update);

            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<List<HireDto>> GetById([FromRoute] int id)
        {
            var hireDto = _hireService.GetById(id);

            return Ok(hireDto);
        }

        [HttpGet]
        public ActionResult<List<HireDto>> GetAll()
        {
            var hireDtos = _hireService.GetAll();

            return Ok(hireDtos);
        }
    }
}
