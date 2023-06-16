using Microsoft.AspNetCore.Mvc;
using RentCarsAPI.Models.Car;
using RentCarsAPI.Models.Client;
using RentCarsAPI.Services;
using System.Collections.Generic;

namespace RentCarsAPI.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            _clientService.Delete(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateClientDto update )
        {
            _clientService.Update(id, update);

            return Ok();
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateClientDto dto)
        {
            int newId=_clientService.Create(dto);

            return Created($"api/cars/{newId}", null);
        }


        [HttpGet("filtr")]
        public ActionResult<IEnumerable<ClientDto>> GetByBlocked([FromHeader] bool IsBlocked)
        {
            var clientsDto = _clientService.GetByBlocked(IsBlocked);

            return Ok(clientsDto);


        }


        [HttpGet]
        public ActionResult<IEnumerable<ClientDto>> GetAll()
        {
            var clientDto=_clientService.GetAll();
            return Ok(clientDto);
        }

        [HttpGet("{id}")]
        public ActionResult<ClientDto> GetById([FromRoute]int id)
        {
            var clientDto = _clientService.GetById( id);
            return Ok(clientDto);
        }

    }
}
