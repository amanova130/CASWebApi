using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;


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

        /// <summary>
        /// Get All Student profile
        /// </summary>
        /// <returns>List of Student</returns>
        [HttpGet("getAllStudents", Name = nameof(GetAllStudents))]
        public ActionResult<List<Student>> GetAllStudents() =>
             _studentService.GetAll();
        
        /// <summary>
        /// Get number of Students
        /// </summary>
        /// <returns>Number of Students</returns>
        [HttpGet("getNumberOfStudents", Name = nameof(GetNumberOfStudents))]
        public ActionResult<int> GetNumberOfStudents() =>
             _studentService.GetNumberOfStudents();
       
        /// <summary>
        /// Get number of Students by class
        /// </summary>
        /// <param name="id">Id of group</param>
        /// <returns>Number of student in specific group</returns>
        [HttpGet("getNumberOfStudentsInClass", Name = nameof(GetNumberOfStudentsInClass))]
        public ActionResult<int> GetNumberOfStudentsInClass(string id) =>
             _studentService.GetNumberOfStudentsByClass(id);
       
        /// <summary>
        /// Get Student profile by Id
        /// </summary>
        /// <param name="id">Id of Student</param>
        /// <returns>Student profile</returns>
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
        

        /// <summary>
        /// Create a new student profile
        /// </summary>
        /// <param name="student">New student object</param>
        /// <returns>Created new student profile</returns>
        [HttpPost("createStudent", Name = nameof(CreateStudent))]
        public ActionResult<Student> CreateStudent(Student student)
        {
            student.Status = true;

            if (!( _studentService.Create(student)))
                return NotFound("duplicated id or wrong id format");
           
            return CreatedAtRoute("getStudentById", new { id = student.Id }, student);
        }

        /// <summary>
        /// Update existed student profile
        /// </summary>
        /// <param name="studentIn">Student profile to update </param>
        /// <returns>Updated student profile</returns>
        [HttpPut("updateStudent", Name = nameof(UpdateStudent))]
        public IActionResult UpdateStudent(Student studentIn)
        {
            bool updated = false;

            var student = _studentService.GetById(studentIn.Id);

            if (student == null)
            {
                return NotFound();
            }
           updated= _studentService.Update(studentIn.Id, studentIn);

            return Ok(updated);
        }

        /// <summary>
        /// Delete student profile from Database, status = false
        /// </summary>
        /// <param name="id">Id of Student</param>
        /// <returns>True- deleted, otherwise false</returns>
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
