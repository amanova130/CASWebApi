﻿using System;
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

        [HttpGet("getAllStudents", Name = nameof(GetAllStudents))]
        public ActionResult<List<Student>> GetAllStudents() =>
             _studentService.GetAll();

        [HttpGet("getStudentById", Name = nameof(GetStudentById))]
        public ActionResult<Student> GetStudentById(string id)
        {
            var student = _studentService.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
        


        [HttpPost("createStudent", Name = nameof(CreateStudent))]
        public ActionResult<Student> CreateStudent(Student student)
        {
            student.Status = true;

            if (!( _studentService.Create(student)))
                return NotFound("duplicated id or wrong id format");
           
            return CreatedAtRoute("getStudentById", new { id = student.Id }, student);
        }

        [HttpPut("updateStudent", Name = nameof(UpdateStudent))]
        public IActionResult UpdateStudent(string id, Student studentIn)
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

        [HttpDelete("deleteStudentById", Name = nameof(DeleteStudentById))]
        public IActionResult DeleteStudentById(string id)
        {
            var student = _studentService.GetById(id);
            if (student != null && _studentService.RemoveById(student.Id))
                return Ok(true);
            return NotFound();
        }
    }
}
