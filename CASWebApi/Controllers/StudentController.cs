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
        public ActionResult<List<Student>> GetAllStudents()
        {
            try
            {
                return _studentService.GetAll();
            }
            catch(Exception e)
            {
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// get student list by groupNumber
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>list of students,or error message if not found or was exception</returns>
        [HttpGet("getAllStudentsByGroup", Name = nameof(GetAllStudentsByGroup))]
        public ActionResult<List<Student>> GetAllStudentsByGroup(string groupName)
        {
            try
            {
                if (groupName != null)
                {
                    var students = _studentService.GetAllStudentsByGroup(groupName);
                        return students;
                }
                return BadRequest("groupName was null");

            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }

        }

        /// <summary>
        /// get all students by faculties names
        /// </summary>
        /// <param name="facultyNames"></param>
        /// <returns>list of students,or error message if not found or was exception</returns>
        [HttpGet("GetAllStudentsByFaculties", Name = nameof(GetAllStudentsByFaculties))]
        public ActionResult<List<Student>> GetAllStudentsByFaculties([FromQuery] string[] facultyNames)
        {
            if (facultyNames != null)
            {
                try
                {
                    var students = _studentService.GetAllStudentsByFaculties(facultyNames);
                    return students;
                }
                catch (Exception e)
                {
                    return BadRequest("No connection to database");
                }
            }
            return BadRequest("facultyNames param is null ");




        }

        /// <summary>
        /// get all students by groupName
        /// </summary>
        /// <param name="groupNames"></param>
        /// <returns>list of students</returns>
        [HttpGet("getAllStudentsByGroups", Name = nameof(GetAllStudentsByGroups))]
        public ActionResult<List<Student>> GetAllStudentsByGroups([FromQuery] string[] groupNames)
        {
            if (groupNames != null)
            {
                try
                {
                    var students = _studentService.GetAllStudentsByGroups(groupNames);
                        return students;
                }
                catch (Exception e)
                {
                    return BadRequest("No connection to database");
                }
            }
            return BadRequest("groupNames param is null");

        }


        /// <summary>
        /// Get number of Students
        /// </summary>
        /// <returns>Number of Students</returns>
        [HttpGet("getNumberOfStudents", Name = nameof(GetNumberOfStudents))]
        public ActionResult<int> GetNumberOfStudents()
        {
            try
            {
                return _studentService.GetNumberOfStudents();
            }
            catch
            {
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// Get number of Students by class
        /// </summary>
        /// <param name="id">Id of group</param>
        /// <returns>Number of student in specific group</returns>
        [HttpGet("getNumberOfStudentsInClass", Name = nameof(GetNumberOfStudentsInClass))]
        public ActionResult<int> GetNumberOfStudentsInClass(string id)
        {
            if (id != null)
            {
                try
                {
                    return _studentService.GetNumberOfStudentsByClass(id);
                }
                catch
                {
                    return BadRequest("No connection to database");
                }
            }
            else
                return BadRequest("given id is null");
        }

        /// <summary>
        /// Get Student profile by Id
        /// </summary>
        /// <param name="id">Id of Student</param>
        /// <returns>Student profile</returns>
        [HttpGet("getStudentById", Name = nameof(GetStudentById))]
        public ActionResult<Student> GetStudentById(string id)
        {
            if (id != null)
            {
                try
                {
                    var student = _studentService.GetById(id);
                    if (student == null)
                        return NotFound("student with given id doesn't exists in db");
                    return student;
                }
                catch (Exception e)
                {
                    return BadRequest("No connection to database");
                }
            }
            else
             return BadRequest("given id was null");

        }
        

        /// <summary>
        /// Create a new student profile
        /// </summary>
        /// <param name="student">New student object</param>
        /// <returns>Created new student profile</returns>
        [HttpPost("createStudent", Name = nameof(CreateStudent))]
        public ActionResult<Student> CreateStudent(Student student)
        {
            if (student != null)
            {
                try
                {
                    _studentService.Create(student);               
                       return CreatedAtRoute("getStudentById", new { id = student.Id }, student);       
                }
                catch (Exception e)
                {            
                    return Conflict(e);
                }
            }
            else
                return BadRequest("Student model is not correct");
            
        }


        /// <summary>
        /// Create a new student profile
        /// </summary>
        /// <param name="student">New student object</param>
        /// <returns>Created new student profile</returns>
        [HttpPost("insertListOfStudents", Name = nameof(InsertListOfStudents))]
        public ActionResult<Student> InsertListOfStudents(List<Student> students)
        {
            if (students != null)
            {
                try
                {
                    if (!(_studentService.InsertManyStudents(students)))
                        return NotFound(false);
                    return Ok(true);
                }
                catch (Exception e)
                {
                    return Conflict(e);
                }
            }
            else
                return BadRequest("given list of students was null");
        }

        /// <summary>
        /// Update existed student profile
        /// </summary>
        /// <param name="studentIn">Student profile to update </param>
        /// <returns>Updated student profile</returns>
        [HttpPut("updateStudent", Name = nameof(UpdateStudent))]
        public IActionResult UpdateStudent(Student studentIn)
        {
            if (studentIn.Id != null || studentIn != null)
            {
                try
                {
                    bool res = false;
                    var student = _studentService.GetById(studentIn.Id);

                    if (student == null)
                    {
                        return NotFound();
                    }
                    res = _studentService.Update(studentIn.Id, studentIn);
                    return Ok(res);
                }
                catch(Exception e)
                {
                    return BadRequest("No connection to database");
                }
            }
            else
                return BadRequest("Id of given student,or student object itself is null");
        }

        /// <summary>
        /// Delete student profile from Database, status = false
        /// </summary>
        /// <param name="id">Id of Student</param>
        /// <returns>True- deleted, otherwise false</returns>
        [HttpDelete("deleteStudentById", Name = nameof(DeleteStudentById))]
        public IActionResult DeleteStudentById(string id)
        {
            if (id != null)
            {
                try
                {
                    var student = _studentService.GetById(id);
                    if (student != null && _studentService.RemoveById(student.Id))
                        return Ok(true);
                    return NotFound("student object with given id doesn't exists");
                }
                catch (Exception e)
                {
                    return BadRequest("No Connection to database");
                }
            }
            else
            {
                return BadRequest("given id was null");
            }
        }
    }
}
