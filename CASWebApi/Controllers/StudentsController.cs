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
    public class StudentsController : ControllerBase
    {
        IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/<StudentsController>
        [HttpGet]
        public List<Student> Get()
        {
            return _studentService.Gets();
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return _studentService.Get(id);
        }

        // POST api/<StudentsController>
        [HttpPost]
        public List<Student> Post([FromBody] Student student)
        {
            return _studentService.Save(student);
        }




        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public List<Student> Delete(int id)
        {
            return _studentService.Delete(id);
        }
    }
}
