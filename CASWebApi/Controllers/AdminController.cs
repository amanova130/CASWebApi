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

        [HttpGet]
        public ActionResult<List<Admin>> Get() =>
             _adminService.GetAll();

        [HttpGet("{id:length(9)}", Name = "GetAdmin")]
        public ActionResult<Admin> Get(string id)
        {
            var admin = _adminService.GetById(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        [HttpPost]
        public ActionResult<Admin> Create(Admin admin)
        {
            _adminService.Create(admin);

            return CreatedAtRoute("GetStudent", new { id = admin.Id }, admin);
        }

        [HttpPut("{id:length(9)}")]
        public IActionResult Update(string id, Admin adminIn)
        {
            var admin = _adminService.GetById(id);

            if (admin == null)
            {
                return NotFound();
            }
            adminIn.Id = id;

            _adminService.Update(id, adminIn);

            return NoContent();
        }

        [HttpDelete("{id:length(9)}")]
        public IActionResult Delete(string id)
        {
            var admin = _adminService.GetById(id);

            if (admin == null)
            {
                return NotFound();
            }

            _adminService.RemoveById(admin.Id);

            return NoContent();
        }
    }
}
