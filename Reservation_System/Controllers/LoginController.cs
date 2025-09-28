using Microsoft.AspNetCore.Mvc;
using Reservation_System.DTO;
using Reservation_System.Entities;
using Reservation_System.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Reservation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginServices _loginServices;
       public LoginController(LoginServices  loginServices) {

            _loginServices = loginServices;
       }

        [HttpPost]

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            
            var result = _loginServices.Login(loginDto);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                return BadRequest(new { message = "Validation failed", errors });
            }
            var user = (User)result.Result;

            var userDto = new UserDto
            {
                userId = user.UserId,
                userName = user.UserName,
                phone = user.Phone,
                mail = user.Mail,
                role = user.Role ?? "User" // default
            };

            return Ok(new ApiResponse<UserDto>
            {
                Result = userDto,
                HasError = false,
                Message = "Login başarılı"
            });
        }

    }
}
