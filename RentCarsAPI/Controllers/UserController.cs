using Microsoft.AspNetCore.Mvc;
using RentCarsAPI.Models.User;
using RentCarsAPI.Services;

namespace RentCarsAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        [HttpPut("{email}")]
        public ActionResult Update([FromRoute] string email, [FromBody] UpdateUserDto dto)
        {
            _userService.Update(email, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _userService.Delete(id);

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetById([FromRoute] int id)
        {
            var user = _userService.GetById(id);

            return Ok(user);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateUserDto userDto)
        {
            var newId = _userService.Create(userDto);

            return Created($"api/users/{newId}", null);
        }
    }
}
