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
    public class StudentController : ControllerBase
    {
        IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public ActionResult<List<Student>> Get() =>
             _studentService.GetAll();

        [HttpGet("{id:length(9)}", Name = "GetStudent")]
        public ActionResult<Student> Get(string id)
        {
            var student = _studentService.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
        


        [HttpPost]
        public ActionResult<Student> Create(Student student)
        {
            student.Status = true;

            if (!( _studentService.Create(student)))
                return NotFound("duplicated id or wrong id format");
           
            return CreatedAtRoute("GetStudent", new { id = student.Id }, student);
        }

        [HttpPut("{id:length(9)}")]
        public IActionResult Update(string id, Student studentIn)
        {
            bool updated = false;

            var student = _studentService.GetById(id);

            if (student == null)
            {
                return NotFound();
            }
            studentIn.Id = id;

           updated= _studentService.Update(id, studentIn);

            return Ok(updated);
        }

        [HttpDelete("{id:length(9)}")]
        public IActionResult Delete(string id)
        {
            var student = _studentService.GetById(id);
            if (student != null && _studentService.RemoveById(student.Id))
                return Ok(true);
            return NotFound();
        }
    }
}
