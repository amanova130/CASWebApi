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
    public class teachersController : ControllerBase
    {
        ITeacherService _teacherService;
        public teachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public ActionResult<List<Teacher>> Get() =>
             _teacherService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetTeacher")]
        public ActionResult<Teacher> Get(string id)
        {
            var teacher = _teacherService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        [HttpPost]
        public ActionResult<Teacher> Create(Teacher teacher)
        {
            _teacherService.Create(teacher);

            return CreatedAtRoute("GetTeacher", new { id = teacher.Id }, teacher);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Teacher teacherIn)
        {
            var teacher = _teacherService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }
            teacherIn.Id = id;

            _teacherService.Update(id, teacherIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var teacher = _teacherService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            _teacherService.RemoveById(teacher.Id);

            return NoContent();
        }
    }
}
