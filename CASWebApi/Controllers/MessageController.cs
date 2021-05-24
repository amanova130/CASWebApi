using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public ActionResult<List<Message>> Get() =>
             _messageService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetMessage")]
        public ActionResult<Message> Get(string id)
        {
            var message = _messageService.GetById(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }



        [HttpPost]
        public ActionResult<Message> Create(Message message)
        {
            if (!(_messageService.Create(message)))
                return NotFound();

            return CreatedAtRoute("GetMessage", new { id = message.Id }, message);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Message messageIn)
        {
            var message = _messageService.GetById(id);

            if (message == null)
            {
                return NotFound();
            }
            messageIn.Id = id;

            _messageService.Update(id, messageIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var message = _messageService.GetById(id);

            if (message != null && _messageService.RemoveById(message.Id))
                return NoContent();
            return NotFound();
        }
    }
}
