using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        ITeacherService _teacherService;
        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("getAllTeachers", Name = nameof(GetAllTeachers))]
        public  ActionResult<List<Teacher>> GetAllTeachers() =>
             _teacherService.GetAll();

        [HttpGet("getTeacherById", Name = nameof(GetTeacherById))]
        public ActionResult<Teacher> GetTeacherById(string id)
        {
            var teacher = _teacherService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        [HttpPost("createTeacher", Name = nameof(CreateTeacher))]
        public ActionResult<Teacher> CreateTeacher(Teacher teacher)
        {
           if(!( _teacherService.Create(teacher)))
                return NotFound("duplicated id or wrong id format");
            return CreatedAtRoute("getTeacherById", new { id = teacher.Id }, teacher);
        }

        [HttpPut("updateTeacher", Name = nameof(UpdateTeacher))]
        public IActionResult UpdateTeacher(Teacher teacherIn)
        {
            bool updated = false;
            var teacher = _teacherService.GetById(teacherIn.Id);

            if (teacher == null)
            {
                return NotFound();
            }
            updated = _teacherService.Update(teacherIn.Id, teacherIn);
        
            return Ok(updated);
        }

        [HttpDelete("deleteTeacherById", Name = nameof(DeleteTeacherById))]
        public IActionResult DeleteTeacherById(string id)
        {
            var teacher = _teacherService.GetById(id);

            if (teacher != null && _teacherService.RemoveById(teacher.Id))
                return Ok(true);
            return NotFound(false);
        }
    }
}
