using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation_System.DTO;
using Reservation_System.Services;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Reservation_System.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationServices _reservationServices;

        public ReservationController(ReservationServices reservationServices)
        {
            _reservationServices = reservationServices;
        }

        [HttpPost]

        public IActionResult SetReservetion([FromBody] ReservationDto Resdto)
        {
            var result = _reservationServices.SetReservation(Resdto);

            if (result == null)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);


        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ReservationDto>>> GetUserReservations(int userId)
        {
            var reservations = await _reservationServices.GetReservationsByUserIdAsync(userId);
            return Ok(reservations);
        }

        // Tüm rezervasyonlar (admin)
        [HttpGet]
        public async Task<ActionResult<List<ReservationDto>>> GetAllReservations()
        {
            var reservations = await _reservationServices.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var result = _reservationServices.DeleteReservation(id);

            if (!result.HasError)
                return NotFound(result);

            return Ok(result);

        }
    }
}
