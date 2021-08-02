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
    public class UserController : ControllerBase
    {
        private readonly ILogger logger;
        IUserService _userService;
        public UserController(IUserService userService, ILogger<FacultyController> logger)
        {
            this.logger = logger;
            _userService = userService;
        }

        [HttpGet("getAllUser", Name = nameof(GetAllUser))]
        public ActionResult<List<User>> GetAllUser()
        {
            logger.LogInformation("Getting all users data");
            var userList = _userService.GetAll();
            if (userList != null)
            {
                logger.LogInformation("Fetched all data");
                return userList;
            }
            else
            {
                logger.LogError("Cannot get access to userList collection in Db");
                return StatusCode(500, "Internal server error");
            }
        }
             

        [HttpGet("getUserById", Name = nameof(GetUserById))]
        public ActionResult<User> GetUserById(string id)
        {
            logger.LogInformation("Getting User by Id");
            if (id != null)
            {
                var user = _userService.GetById(id);
                if (user != null)
                {
                    logger.LogInformation("Got User");
                    return Ok(user);
                }
                else
                {
                    logger.LogError("Cannot get access to user collection in Db");
                }
            }
            else
                logger.LogError("Course Id is null or empty string");
            return BadRequest(null);
        }

        [HttpPost("createUser", Name = nameof(CreateUser))]
        public ActionResult<User> CreateUser(User user)
        {
            logger.LogInformation("Creating a new user");
            if (user != null)
            {
                user.Status = true;
                if (_userService.Create(user) != null)
                    return CreatedAtRoute("getUserById", new { id = user.UserName }, user);
                else
                    logger.LogError("Duplicated Id");
            }
            else
                logger.LogError("Faculty object is null " + user);
            return BadRequest(null); 
        }

        [HttpPut("updateUser", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(string id, User userIn)
        {
            logger.LogInformation("Updating existed faculty: " + userIn.UserName);
            if (userIn != null)
            {
                var user = _userService.GetById(id);

                if (user != null)
                {
                    userIn.UserName = id;
                    _userService.Update(id, userIn);
                    logger.LogInformation("Given Faculty profile Updated successfully");
                    return Ok(true);
                }
                else
                    logger.LogError("User  with Name: " + userIn.UserName + " doesn't exist");

            }
            else
                logger.LogError("CourseIn objest is null");
            return BadRequest(false);
        }


        [HttpDelete("deleteUserById", Name = nameof(DeleteUserById))]
        public IActionResult DeleteUserById(string id)
        {
            logger.LogInformation("Deleting user by Id");
            if(id != null)
            {
                var user = _userService.GetById(id);
                if (user != null && _userService.RemoveById(user.UserName))
                    return Ok(true);
                else
                    logger.LogError("Cannot get access to user collection in Db");
            }
            else
                logger.LogError("Id is not valid format or null");
            return NotFound(false);
        }
    }
}
