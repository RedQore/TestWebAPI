using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Models;
using TestWebAPI.Services;

namespace TestWebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly MessageQueue _messageQueue;

        public MessagesController(ILogger<MessagesController> logger, MessageQueue messageQueue)
        {
            _logger = logger;
            _messageQueue = messageQueue;
        }

        [HttpGet("All")]
        public IEnumerable<ServerMessage> GetAll()
        {
            return _messageQueue.GetAll();
        }

        [HttpGet(Name = "Messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int recipientId)
        {
            var message = _messageQueue.GetById(recipientId);

            return message.Any() ? Ok(message) : NotFound();
        }


        [HttpDelete(Name = "Messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteById(int recipientId)
        {
            var message = _messageQueue.Pop(recipientId);

            return message.Any() ? Ok(message) : NotFound();
        }

        [HttpPost(Name = "Messages")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(IEnumerable<ServerMessage> messages)
        {
            _messageQueue.Put(messages);

            return CreatedAtAction(
                nameof(Post),
                new { ids = messages.SelectMany(message => message.Recipients).Distinct() },
                messages);
        }
    }
}
