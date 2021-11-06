using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger logger;
        IMessageService _messageService;
        public MessageController(IMessageService messageService, ILogger<MessageController> logger)
        {
            this.logger = logger;
            _messageService = messageService;
        }

        [HttpGet("getAllMsg", Name = nameof(GetAllMsg))]
        public ActionResult<List<Message>> GetAllMsg()
        {
            try
            {
                var messages = _messageService.GetAll();
                logger.LogInformation("Fetched All messages data");
                return messages;
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }

        [HttpGet("getAllMsgByReceiver", Name = nameof(getAllMsgByReceiver))]
        public ActionResult<List<Message>> getAllMsgByReceiver(string id)
        {
            if(id == null || id == "")
            {
                logger.LogError("message Id is null or empty string");
                return BadRequest("Incorrect format of Id param");
            }
            try
            {
                var messages = _messageService.GetAllByReceiverId(id);
                logger.LogInformation("Fetched All messages data by receiver id");
                return messages;
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }

        [HttpGet("getAllMsgBySender", Name = nameof(getAllMsgBySender))]
        public ActionResult<List<Message>> getAllMsgBySender(string id)
        {
            if (id == null || id == "")
            {
                logger.LogError("message Id is null or empty string");
                return BadRequest("Incorrect format of Id param");
            }
            try
            {
                var messages = _messageService.GetAllBySenderId(id);
                return messages;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("getAllDeletedBySender", Name = nameof(GetAllDeletedBySender))]
        public ActionResult<List<Message>> GetAllDeletedBySender(string id)
        {
            if (id == null || id == "")
            {
                logger.LogError("message Id is null or empty string");
                return BadRequest("Incorrect format of Id param");
            }
            try
            {
                var messages = _messageService.GetAllDeletedBySender(id);
                logger.LogInformation("Fetched All deleted messages data by sender id");
                return messages;
            }
            catch (Exception e)

            {
                return BadRequest("No connection to database");
            }
        }
        

        [HttpGet("getMsgById", Name = nameof(GetMsgById))]
        public ActionResult<Message> GetMsgById(string id)
        {
            if (id == null || id == "")
            {
                logger.LogError("message Id is null or empty string");
                return BadRequest("Incorrect format of Id param");
            }
            try
            {
                var message = _messageService.GetById(id);
                logger.LogInformation("Fetched message by id");
                return message;
            }
            catch (Exception e)

            {
                return BadRequest("No connection to database");
            }




        }


        [HttpPost("sendEmail", Name = nameof(SendMail))]
        public  IActionResult SendMail([FromBody] Message email)
        {
            if(email == null)
            {
                logger.LogError("email object is null");
                return BadRequest("Incorrect format of email param");
            }
            try
            {
                _messageService.Create(email);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }
        [HttpPost("createMsg", Name = nameof(CreateMsg))]
        public ActionResult<Message> CreateMsg(Message message)
        {
            if (message == null)
            {
                logger.LogError("email object is null");
                return BadRequest("Incorrect format of email param");
            }
            try
            {
                _messageService.Create(message);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }

        }


        [HttpDelete("deleteMsgById", Name = nameof(DeleteMsgById))]
        public IActionResult DeleteMsgById(string id)
        {
            if (id == null)
            {
                logger.LogError("id is null");
                return BadRequest("Incorrect format of id param");
            }
            try
            {
                var message = _messageService.GetById(id);
                if (message != null && _messageService.RemoveById(message.Id))
                    return Ok(true);
                return NotFound("message with given id not found");
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }

        }
    }
}
