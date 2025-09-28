using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation_System.DTO;
using Reservation_System.Services;

namespace Reservation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly  UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
            
        }

        [HttpPost]

        public IActionResult SetUser([FromBody] UserDto userDto)
        {
            var result= _userService.SetUser(userDto);

            if (result == null) {
                return BadRequest(result.Message);
            }


            return Ok(result);
        }

        [HttpGet]

        public IActionResult GetUsers()
        {
            var result = _userService.GetUsers();

            if (result == null) { 
            
            return BadRequest(result.Message);
            }
            return Ok(result);
        }

     





    }
}
