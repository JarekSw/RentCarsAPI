using Microsoft.AspNetCore.Mvc;
using RentCarsAPI.Models.Hire;
using RentCarsAPI.Services;
using System.Collections.Generic;

namespace RentCarsAPI.Controllers
{
    [ApiController]
    [Route("api/hires")]
    public class HireController:ControllerBase
    {
        private readonly IHireService _hireService;

        public HireController(IHireService service)
        {
              _hireService=service;
        }

        [HttpGet]
        public ActionResult<List<HireDto>> GetAll()
        {
            var hireDtos=_hireService.GetAll();

            return Ok(hireDtos);
        }



    }
}
