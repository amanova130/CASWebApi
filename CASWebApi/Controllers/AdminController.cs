using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;


namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IAdminService _adminService;
        IUserService _userService;

        public AdminController(IAdminService adminService, IUserService userService)

        {
            _adminService = adminService;
            _userService = userService;

        }

        /// <summary>
        /// Function to get all Admin data
        /// </summary>
        /// <returns>List of admins</returns>
        [HttpGet("getAllAdmin", Name = nameof(GetAllAdmin))]
        public ActionResult<List<Admin>> GetAllAdmin() =>
             _adminService.GetAll();

        /// <summary>
        /// Get Admin data by Id
        /// </summary>
        /// <param name="id">Admin Id</param>
        /// <returns>Object that contains Admin data</returns>
        [HttpGet("getAdminById", Name = nameof(GetAdminById))]
        public ActionResult<Admin> GetAdminById(string id)
        {
            var admin = _adminService.GetById(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        /// <summary>
        /// Function to create a new Admin profile
        /// </summary>
        /// <param name="admin">Object that contain Admin profile </param>
        /// <returns>Created Admin profile</returns>
        [HttpPost("createNewAdmin", Name = nameof(CreateNewAdmin))]
        public ActionResult<Admin> CreateNewAdmin(Admin admin)
        {
            User _user = new User();
            admin.Status = true;
            _user.UserName = admin.Id;
            _user.Password = admin.Birth_date.Replace("-", "");
            _user.Role = "Admin";
            if (!(_adminService.Create(admin)))
                return NotFound("duplicated id or wrong id format");
            _userService.Create(_user);
            return CreatedAtRoute("getAdminById", new { id = admin.Id }, admin);
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
            var admin = _adminService.GetById(id);

            if (admin == null)
            {
                return NotFound();
            }
            adminIn.Id = id;

            if (!(_adminService.Update(id, adminIn)))
                 return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Delete Admin by Id
        /// </summary>
        /// <param name="id"> Admin Id</param>
        /// <returns></returns>
        [HttpDelete("deleteAdminById", Name = nameof(DeleteAdminById))]

        public IActionResult DeleteAdminById(string id)
        {
            var admin = _adminService.GetById(id);
            if (admin != null && _adminService.RemoveById(admin.Id))
                return NoContent();
            return NotFound();

        }
    }
}
