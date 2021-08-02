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
        IGroupService _groupService;
        IUserService _userService;
        public StudentController(IStudentService studentService, IUserService userService, IGroupService groupService)
        {
            _studentService = studentService;
            _userService = userService;
            _groupService = groupService;
        }

        /// <summary>
        /// Get All Student profile
        /// </summary>
        /// <returns>List of Student</returns>
        [HttpGet("getAllStudents", Name = nameof(GetAllStudents))]
        public ActionResult<List<Student>> GetAllStudents() =>
             _studentService.GetAll();

        [HttpGet("getAllStudentsByGroup", Name = nameof(GetAllStudentsByGroup))]

        public ActionResult<List<Student>> GetAllStudentsByGroup( string groupName)
        {

            var students = _studentService.GetAllStudentsByGroup(groupName);

            if (students == null)
            {
                return NotFound();
            }


            return students;

        }

        [HttpGet("GetAllStudentsByFaculties", Name = nameof(GetAllStudentsByFaculties))]
        public ActionResult<List<Student>> GetAllStudentsByFaculties([FromQuery] string[] facultyNames)
        {
            List<Student> students = new List<Student>();
            List<Group> groups = new List<Group>();


            for (int i = 0; i < facultyNames.Length; i++)
                groups.AddRange(_groupService.GetGroupsByFaculty(facultyNames[i]));
            for (int i = 0; i < groups.Count; i++)
                students.AddRange(_studentService.GetAllStudentsByGroup(groups[i].GroupNumber));
            return students;


        }
        [HttpGet("getAllStudentsByGroups", Name = nameof(GetAllStudentsByGroups))]
        public ActionResult<List<Student>> GetAllStudentsByGroups([FromQuery] string[] groupNames)
        {
            List<Student> students = new List<Student>();
                for (int i = 0; i < groupNames.Length; i++)
                    students.AddRange(_studentService.GetAllStudentsByGroup(groupNames[i]));
            return students;


        }


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
            if (student != null)
            {
                student.Status = true;
                if (student.Image == null || student.Image == "")
                    student.Image = "Resources/Images/noPhoto.png";
                User _user = new User();
                _user.UserName = student.Id;
                _user.Password = student.Birth_date.Replace("-", "");
                _user.Role = "Student";

                if (_studentService.Create(student))
                {
                    _userService.Create(_user);
                    return CreatedAtRoute("getStudentById", new { id = student.Id }, student);
                }
                else
                    return NotFound("duplicated id or wrong id format");
            }
            else
                return BadRequest(null);
            
        }


        /// <summary>
        /// Create a new student profile
        /// </summary>
        /// <param name="student">New student object</param>
        /// <returns>Created new student profile</returns>
        [HttpPost("insertListOfStudents", Name = nameof(InsertListOfStudents))]
        public ActionResult<Student> InsertListOfStudents(List<Student> students)
        {
            students.ForEach(student =>
            {
                student.Status = true;
                student.Image = "Resources/Images/noPhoto.png";
                User _user = new User();
                _user.UserName = student.Id;
                _user.Password = student.Birth_date.Replace("-", "");
                _user.Email = student.Email;
                _user.Role = "Student";
                _user.Status = true;
                _userService.Create(_user);
            });
            if (!(_studentService.InsertManyStudents(students)))
                return NotFound("duplicated id or wrong id format");

            return Ok(true);
        }

        /// <summary>
        /// Update existed student profile
        /// </summary>
        /// <param name="studentIn">Student profile to update </param>
        /// <returns>Updated student profile</returns>
        [HttpPut("updateStudent", Name = nameof(UpdateStudent))]
        public IActionResult UpdateStudent(Student studentIn)
        {
            bool updatedStudent = false;
            bool updatedUser = false ;

            var student = _studentService.GetById(studentIn.Id);

            if (student == null)
            {
                return NotFound();
            }
            updatedStudent = _studentService.Update(studentIn.Id, studentIn);
            if(updatedStudent)
            {
               var user = _userService.GetById(studentIn.Id);
                if (user.Email != studentIn.Email)
                {
                    user.Email = studentIn.Email;
                    _userService.Update(user.UserName, user);
                }
            }

            return Ok(updatedStudent && updatedUser);
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
            if (student != null && _studentService.RemoveById(student.Id) && _userService.RemoveById(student.Id))

                return Ok(true);
            
            return NotFound();
        }
    }
}
