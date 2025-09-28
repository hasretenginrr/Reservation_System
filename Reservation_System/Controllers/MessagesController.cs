using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation_System.DTO;
using Reservation_System.Services;

namespace Reservation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessagesServices _messagesService;

        public MessagesController(MessagesServices messageService)
        {
            _messagesService = messageService;
            
        }

        [HttpPost]
        public IActionResult SetMessages([FromBody] MessagesDto messagesDto)
        {
            var result= _messagesService.SetMessages(messagesDto);
            if(result== null)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);

        }
    }
}
