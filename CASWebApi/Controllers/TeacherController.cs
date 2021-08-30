using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ILogger logger;
        ITeacherService _teacherService;
        public TeacherController(ITeacherService teacherService, ILogger<TeacherController> logger)
        {
            this.logger = logger;
            _teacherService = teacherService;
        }


        /// <summary>
        /// a function to get list of all teacher from db
        /// </summary>
        /// <returns>list of teachers</returns>
        [HttpGet("getAllTeachers", Name = nameof(GetAllTeachers))]
        public  ActionResult<List<Teacher>> GetAllTeachers() 
        {
            logger.LogInformation("Getting all Teachers from TeacherController");
            try
            {
                var teacherList = _teacherService.GetAll();
                logger.LogInformation("Fetched All teacher data");
                return teacherList;
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to Db");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// get total number of teachers in db
        /// </summary>
        /// <returns>number of teachers</returns>
        [HttpGet("getNumberOfTeachers", Name = nameof(GetNumberOfTeachers))]
        public ActionResult<int> GetNumberOfTeachers() =>
          _teacherService.GetNumberOfTeachers();

        [HttpGet("getTeacherById", Name = nameof(GetTeacherById))]
        public ActionResult<Teacher> GetTeacherById(string id)
        {
            logger.LogInformation("Getting Teacher by given Id from TeacherController");
            try
            {
                var teacher = _teacherService.GetById(id);
                if (teacher == null)
                {
                    logger.LogError("Cannot get access to teacher collection in Db");
                }
                logger.LogInformation("Fetched teacher data by id");

                return teacher;
            }
            catch(Exception e)
            {
                logger.LogError("Cannot get access to Db");
                return StatusCode(500, "Internal server error");
            }
          
        }

        /// <summary>
        /// get number of teachers by courseName
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns>number of teachers</returns>
        [HttpGet("getTeacherByCourse", Name = nameof(GetTeachersByCourseName))]
        public ActionResult<List<Teacher>> GetTeachersByCourseName(string courseName)
        {
            logger.LogInformation("Getting Teacher by given courseName from teacherController");
            try
            {
                var teacherList = _teacherService.GetTeachersByCourseName(courseName);
                if (teacherList == null)
                {
                    logger.LogError("Cannot get access to teacher collection in Db");
                }
                logger.LogInformation("Fetched teacher data by courseName");
                return teacherList;
            }
            catch(Exception e)
            {
                logger.LogError("Cannot get access to Db");
                return StatusCode(500, "Internal server error");
            }
           
        }

        /// <summary>
        /// add new teacher object to db
        /// </summary>
        /// <param name="teacher">teacher object to add</param>
        /// <returns> teacher object if added,otherwise null </returns>
        [HttpPost("createTeacher", Name = nameof(CreateTeacher))]
        public ActionResult<Teacher> CreateTeacher(Teacher teacher)
        {
            logger.LogInformation("Creating a new teacher profile: " + teacher);
            if (teacher != null)
            {
                try
                {
                    _teacherService.Create(teacher);
                    return CreatedAtRoute("getTeacherById", new { id = teacher.Id }, teacher);
                }              
                catch(Exception e)
                {
                    logger.LogError("Cannot create a teacher, duplicated id or wrong format");
                    return Conflict(e);
                }

            }
            logger.LogError("A new Teacher model is not valid");
            return BadRequest("Teacher param or his Id is null ");
        }

        /// <summary>
        /// update existed teacher profile
        /// </summary>
        /// <param name="teacherIn">id of the teacher to update</param>
        /// <returns>true if updated,otherwise false</returns>
        [HttpPut("updateTeacher", Name = nameof(UpdateTeacher))]
        public IActionResult UpdateTeacher(Teacher teacherIn)
        {
            logger.LogInformation("Updating existed teacher profile: " +teacherIn.Id);
            if(teacherIn != null && teacherIn.Id != null)
            {
                try
                {
                    var teacher = _teacherService.GetById(teacherIn.Id);
                    if (teacher == null)
                    {
                        logger.LogError("Teacher with Id: " + teacherIn.Id + " doesn't exist");
                        return NotFound(false);
                    }
                    bool updated = _teacherService.Update(teacherIn.Id, teacherIn);
                    if (updated)
                        logger.LogInformation("Given Teacher profile Updated successfully");
                    else
                        logger.LogError("Cannot update the teacher profile: " + teacherIn.Id + " wrong format");

                    return Ok(updated);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            logger.LogError("A given Teacher model is not valid");
            return BadRequest("Teacher param or Id is null ");
        }

        /// <summary>
        /// deletes teacher profile from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if deleted,otherwise false</returns>
        [HttpDelete("deleteTeacherById", Name = nameof(DeleteTeacherById))]
        public IActionResult DeleteTeacherById(string id)
        {
            if(id != null)
            {
                try
                {
                    logger.LogInformation("Deleting existed teacher profile: " + id);
                    var teacher = _teacherService.GetById(id);

                    if (teacher != null && _teacherService.RemoveById(teacher.Id))
                    {
                        logger.LogInformation("Teacher profile has been deleted successfully " + teacher);
                        return Ok(true);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest("No connection to database");
                }
            }
            logger.LogError("Id is not valid or empty string");
            return NotFound("Id is not valid or empty string");
        }
    }
}
