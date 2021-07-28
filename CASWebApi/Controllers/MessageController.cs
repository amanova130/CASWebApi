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

        [HttpGet("getAllMsg", Name = nameof(GetAllMsg))]
        public ActionResult<List<Message>> GetAllMsg() =>
             _messageService.GetAll();

        [HttpGet("getAllMsgByReceiver", Name = nameof(getAllMsgByReceiver))]
        public ActionResult<List<Message>> getAllMsgByReceiver(string id) =>
             _messageService.GetAllByReceiverId(id);

        [HttpGet("getAllMsgBySender", Name = nameof(getAllMsgBySender))]
        public ActionResult<List<Message>> getAllMsgBySender(string id) =>
            _messageService.GetAllBySenderId(id);

        [HttpGet("getMsgById", Name = nameof(GetMsgById))]
        public ActionResult<Message> GetMsgById(string id)
        {
            var message = _messageService.GetById(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }


        [HttpPost("sendEmail", Name = nameof(SendMail))]
        public async Task<IActionResult> SendMail([FromBody] Message email)
        {
            var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;

            client.Credentials = new System.Net.NetworkCredential("casmanagment78@gmail.com", "casmanagment1337");

            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress("casmanagment78@gmail.com");
            string emailReceivers = String.Empty;
            for (int i = 0; i < email.Receiver.Length; i++)
                emailReceivers += email.Receiver[i].Email+',';
            emailReceivers = emailReceivers.TrimEnd(',');
            email.Sender = "Admin";
            mailMessage.To.Add(emailReceivers);
            
            
            mailMessage.Body = email.Description;

            mailMessage.Subject = email.Subject;

            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

            await client.SendMailAsync(mailMessage);
            _messageService.Create(email);

            return Ok();
        }
        [HttpPost("createMsg", Name = nameof(CreateMsg))]
        public ActionResult<Message> CreateMsg(Message message)
        {
            if (!(_messageService.Create(message)))
                return NotFound();

            return CreatedAtRoute("getMsgById", new { id = message.Id }, message);
        }

        [HttpPut("updateMsg", Name = nameof(UpdateMsg))]
        public IActionResult UpdateMsg(string id, Message messageIn)
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

        [HttpDelete("deleteMsgById", Name = nameof(DeleteMsgById))]
        public IActionResult DeleteMsgById(string id)
        {
            var message = _messageService.GetById(id);

            if (message != null && _messageService.RemoveById(message.Id))
                return NoContent();
            return NotFound();
        }
    }
}
