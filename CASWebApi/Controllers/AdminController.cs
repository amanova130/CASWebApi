using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("getAllAdmin", Name = nameof(GetAllAdmin))]
        public ActionResult<List<Admin>> GetAllAdmin() =>
             _adminService.GetAll();

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

        [HttpPost("createNewAdmin", Name = nameof(CreateNewAdmin))]
        public ActionResult<Admin> CreateNewAdmin(Admin admin)
        {
            if (!(_adminService.Create(admin)))
                return NotFound("duplicated id or wrong id format");
            return CreatedAtRoute("GetAdminById", new { id = admin.Id }, admin);
        }
        
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

        [HttpDelete("deleteAdminById", Name = nameof(DeleteAdminById))]

        public IActionResult DeleteAdminById(string id)
        {
            var admin = _adminService.GetById(id);
            if (admin != null && _adminService.RemoveById(admin.Id))
                return NoContent();
            return NotFound();

           /* if (admin == null)
            {
                return NotFound();
            }

           if(!( _adminService.RemoveById(admin.Id)))
                 return NotFound();
           return NoContent();*/
        }
    }
}
