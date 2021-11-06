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

        public UserController(IUserService userService,ILogger<UserController> logger)
        {
            this.logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Check entered user name and password
        /// </summary>
        /// <param name="pass">Password</param>
        /// <param name="userId">UserName</param>
        /// <returns>Result or exception if something went wrong</returns>
        [HttpGet("checkEnteredPWD", Name = nameof(CheckEnteredPWD))]
        public ActionResult<bool> CheckEnteredPWD(string pass,string userId)
        {
            try
            {
                bool res = _userService.checkEnteredPass(pass, userId);
                if (!res)
                    return BadRequest("Username and password not correct");
                return res;
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to userList collection in Db");
                return StatusCode(500, "Internal server error");
            } 
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns>List of Users</returns>
        [HttpGet("getAllUser", Name = nameof(GetAllUser))]
        public ActionResult<List<User>> GetAllUser()
        {
            logger.LogInformation("Getting all users data");
            try
            {
                var userList = _userService.GetAll();
                logger.LogInformation("Fetched all data");
                return userList;
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to userList collection in Db");
                return StatusCode(500, "Internal server error");
            }
        }
             
        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id">Id of User</param>
        /// <returns>User object</returns>
        [HttpGet("getUserById", Name = nameof(GetUserById))]
        public ActionResult<User> GetUserById(string id)
        {
            logger.LogInformation("Getting User by Id");
            if (id != null && id != "")
            {
                try
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
                catch(Exception e)
                {
                    logger.LogError("Cannot get access to Db");
                    return StatusCode(500, "Internal server error");
                }
            }
            else
                logger.LogError("Course Id is null or empty string");
            return NotFound("Id param is null or empty string");
        }

        /// <summary>
        /// Check Authentication
        /// </summary>
        /// <param name="userToCheck"></param>
        /// <returns></returns>
        [HttpPost("checkAuth", Name = nameof(CheckAuth))]
        public ActionResult<User> CheckAuth(User userToCheck)
        {
            logger.LogInformation("Getting User by Id");
            try
            {
                if (userToCheck.UserName != null && userToCheck.Password != null)
                {
                    var user = _userService.checkAuth(userToCheck);
                    if (user != null)
                    {
                        logger.LogInformation("Got User");
                        return Ok(user);
                    }
                    else
                    {
                        logger.LogError("Cannot get access to user collection in Db");
                        return NotFound("User not found");
                    }
                }
                else
                    logger.LogError("User Id is null or empty string");
                return BadRequest("Id param is null or empty string");
            }
            catch(Exception e)
            {
                logger.LogError("Cannot get access to Db");
                return StatusCode(500, "Internal server error");
            } 
        }

        /// <summary>
        /// Get user by Email to change password
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User object</returns>
        [HttpGet("getUserByEmail", Name = nameof(getUserByEmail))]
        public ActionResult<User> getUserByEmail(string email)
        {
            try
            {
                var user = _userService.getByEmail(email);
                if (user != null)
                {
                    return user;
                }
                else
                    return NotFound("User email not found ");
            }
           catch(Exception e)
            {
                return BadRequest("No connection to database");
            }   
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Result</returns>
        [HttpGet("resetPass", Name = nameof(ResetPass))]
        public ActionResult<bool> ResetPass([FromQuery] string email)
        {
            try
            {
                bool res = _userService.resetPass(email);
                if (res)
                    return Ok(res);
                else
                    return NotFound("cannot reset password");
            }
            catch(Exception e)
            {
                logger.LogError("Cannot get access to Db");
                return StatusCode(500, "Internal server error");
            }
           
        }

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User object</returns>
        [HttpPost("createUser", Name = nameof(CreateUser))]
        public ActionResult<User> CreateUser(User user)
        {
            logger.LogInformation("Creating a new user");
            if (user != null)
            {
                try
                {
                    _userService.Create(user);
                    return CreatedAtRoute("getUserById", new { id = user.UserName }, user);
                }
                catch(Exception e)
                {
                    logger.LogError("Duplicated Id: "+user.UserName);
                    return Conflict(e);
                }
                    
            }
            else
                logger.LogError("User object is null " + user);
            return BadRequest("User object not correct"); 
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="userIn"></param>
        /// <returns>Result</returns>
        [HttpPut("updateUser", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(User userIn)
        {
            logger.LogInformation("Updating existed User: " + userIn.UserName);
            if (userIn != null)
            {
                try
                {
                    var user = _userService.GetById(userIn.UserName);
                    if (user != null)
                    {
                        _userService.Update(userIn);
                        logger.LogInformation("Given User profile Updated successfully");
                        return Ok(true);
                    }
                    else
                        logger.LogError("User  with Name: " + userIn.UserName + " doesn't exist");
                }
               catch(Exception e)
                {
                    logger.LogError("Cannot get access to Db");
                    return StatusCode(500, "Internal server error");
                }
            }
            else
                logger.LogError("CourseIn objest is null");
            return BadRequest("CourseIn objest is null");
        }

        /// <summary>
        /// Delete User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Result true if dleted succssefully</returns>
        [HttpDelete("deleteUserById", Name = nameof(DeleteUserById))]
        public IActionResult DeleteUserById(string id)
        {
            logger.LogInformation("Deleting user by Id");
            if(id != null)
            {
                try
                {
                    var user = _userService.GetById(id);
                    if (user != null && _userService.RemoveById(user.UserName))
                        return Ok(true);
                }
                catch(Exception e)
                {
                    return BadRequest("No connection to database");
                }
            }
            else
                logger.LogError("Id is not valid format or null");
            return NotFound("Id is not valid format or null");
        }
    }
}
