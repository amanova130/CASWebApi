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
    public class UserController : ControllerBase
    {
        IUserService _userService;
        IMessageService _messageService;
        public UserController(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        [HttpGet("getAllUser", Name = nameof(GetAllUser))]
        public ActionResult<List<User>> GetAllUser() =>
             _userService.GetAll();

        [HttpGet("getUserById", Name = nameof(GetUserById))]
        public ActionResult<User> GetUserById(string id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("getUserByEmail", Name = nameof(getUserByEmail))]
        public ActionResult<User> getUserByEmail(string email)
        {
            var user = _userService.getByEmail(email);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("resetPass", Name = nameof(ResetPass))]
        public ActionResult<bool> ResetPass([FromQuery] string email)
        {
            var user = _userService.getByEmail(email);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.Password = _userService.RandomString(6, true);
                _userService.Update(user.UserName, user);
            }

            Message resetPass = new Message();
            resetPass.Receiver = new string[1];
            resetPass.Receiver[0] = email;
            resetPass.Description = "Following your request, a password reset for the system was performed\n"
                                      + "Your new password is:\n"
                                      + user.Password+"\n"                                    
                                      + "Do not reply to this message.\n"
                                      + "This system message has been sent to you automatically because you have requested a password reset.";

            resetPass.Subject = " Reset password";
            resetPass.DateTime = new DateTime();
            resetPass.status = true;
           bool res = _messageService.Create(resetPass);
          

            return res;
        }

        [HttpPost("createUser", Name = nameof(CreateUser))]
        public ActionResult<User> CreateUser(User user)
        {
            _userService.Create(user);

            return CreatedAtRoute("getUserById", new { id = user.UserName }, user);
        }

        [HttpPut("updateUser", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(string id, User userIn)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            userIn.UserName = id;

            _userService.Update(id, userIn);

            return NoContent();
        }

        [HttpDelete("deleteUserById", Name = nameof(DeleteUserById))]
        public IActionResult DeleteUserById(string id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.RemoveById(user.UserName);

            return NoContent();
        }
    }
}
