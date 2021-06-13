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
        public UserController(IUserService userService)
        {
            _userService = userService;
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

        [HttpPost("createUser", Name = nameof(CreateUser))]
        public ActionResult<User> CreateUser(User user)
        {
            _userService.Create(user);

            return CreatedAtRoute("getUserById", new { id = user.Id }, user);
        }

        [HttpPut("updateUser", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(string id, User userIn)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            userIn.Id = id;

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

            _userService.RemoveById(user.Id);

            return NoContent();
        }
    }
}
