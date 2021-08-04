using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        //Logger to create streammer of logs 
        private readonly ILogger logger;
        IAdminService _adminService;

        public AdminController(IAdminService adminService, IUserService userService, ILogger<AdminController> logger)
        {
            this.logger = logger;
            _adminService = adminService;

        }

        /// <summary>
        /// Function to get all Admin data
        /// </summary>
        /// <returns>List of admins</returns>
        [HttpGet("getAllAdmin", Name = nameof(GetAllAdmin))]
        public ActionResult<List<Admin>> GetAllAdmin()
        {
            logger.LogInformation("Getting all Admins data");
            var adminList = _adminService.GetAll();
            if (adminList != null)
            {
                logger.LogInformation("Fetched all data");
                 return adminList;
            }  
            else
            {
                logger.LogError("Cannot get access to admin collection in Db");
                return StatusCode(500, "Internal server error");
            }   

        }
             

        /// <summary>
        /// Get Admin data by Id
        /// </summary>
        /// <param name="id">Admin Id</param>
        /// <returns>Object that contains Admin data</returns>
        [HttpGet("getAdminById", Name = nameof(GetAdminById))]
        public ActionResult<Admin> GetAdminById(string id)
        {
            logger.LogInformation("Getting Admin data");
            if(id != null && id != "")
            {
                var admin = _adminService.GetById(id);
                if (admin == null)
                {
                    return NotFound();
                }
                else
                {
                    logger.LogInformation("Got Admin by Id" + admin);
                    return Ok(admin);
                }    
            }
            else
            {
                logger.LogError("Id is null or empty string");
                return NotFound();
            }   
        }

        /// <summary>
        /// Function to create a new Admin profile
        /// </summary>
        /// <param name="admin">Object that contain Admin profile </param>
        /// <returns>Created Admin profile</returns>
        [HttpPost("createNewAdmin", Name = nameof(CreateNewAdmin))]
        public ActionResult<Admin> CreateNewAdmin(Admin admin)
        {
            logger.LogInformation("Creating a new Admin profile");
            if(admin != null)
            {
                if (!(_adminService.Create(admin)))
                    return NotFound("duplicated id or wrong id format");
                return CreatedAtRoute("getAdminById", new { id = admin.Id }, admin);
            }
            else
            {
                logger.LogError("A new admin model is not valid");
                return BadRequest(admin);
            }
            
            
        }
        
        /// <summary>
        /// Update existed Admin profile
        /// </summary>
        /// <param name="id">Admin Id</param>
        /// <param name="adminIn">Object that need to update</param>
        /// <returns>Updated Admin profile</returns>
        [HttpPut("updateAdmin", Name = nameof(UpdateAdmin))]
        public IActionResult UpdateAdmin(string id, Admin adminIn)
        {
            logger.LogInformation("Updating existed Admin");
            if(id != null)
            {
                var admin = _adminService.GetById(id);
                if (admin == null)
                {
                    logger.LogError("Admin with id: " + id + " not found");
                    return NotFound();
                }
                adminIn.Id = id;
                if (!(_adminService.Update(id, adminIn)))
                {
                    logger.LogError("Cannot update the admin profile, something went wrong in UpdateAdmin");
                    return NotFound();
                }    
            }
            return Ok();
        }

        /// <summary>
        /// Delete Admin by Id
        /// </summary>
        /// <param name="id"> Admin Id</param>
        /// <returns></returns>
        [HttpDelete("deleteAdminById", Name = nameof(DeleteAdminById))]
        public IActionResult DeleteAdminById(string id)
        {
            logger.LogInformation("Deleting Admin by Id");
            if(id != null)
            {
                var admin = _adminService.GetById(id);
                if (admin != null && _adminService.RemoveById(admin.Id))
                    return Ok();
            }
            logger.LogError("Id is not valid or empty string");
            return NotFound();

        }
    }
}
