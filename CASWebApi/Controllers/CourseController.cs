using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        //Logger to create streammer of logs 
        private readonly ILogger logger;
        ICourseService _courseService;
        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            this.logger = logger;
            _courseService = courseService;
        }

        /// <summary>
        /// Get all courses from Database
        /// </summary>
        /// <returns>List of courses</returns>
        [HttpGet("getAllCourse", Name = nameof(GetAllCourse))]
        public ActionResult<List<Course>> GetAllCourse()
        {
            logger.LogInformation("Getting all Course from CourseController");
            var courseList = _courseService.GetAll();
            if(courseList != null)
            {
                logger.LogInformation("Fetched All course data");
                return Ok(courseList);
            }
            else
            {
                logger.LogError("Cannot get access to course collection in Db");
                return StatusCode(500, "Internal server error");
            }

        }
             

        /// <summary>
        /// Get Number of Courses
        /// </summary>
        /// <returns>Number of Courses</returns>
        [HttpGet("getNumberOfCourses", Name = nameof(getNumberOfCourses))]
        public ActionResult<int> getNumberOfCourses()
        {
            logger.LogInformation("Getting number of Courses");
            var numberOfCourse = _courseService.GetNumberOfCourses();
            if(numberOfCourse > 0)
            {
                return Ok(numberOfCourse);
            }
            else
            {
                logger.LogError("Cannot get access to course collection in Db");
                return StatusCode(500, "Internal server error");
            }
        }
             

        /// <summary>
        /// Get Course profile by Id
        /// </summary>
        /// <param name="id">Id of course</param>
        /// <returns>Course profile</returns>
        [HttpGet("getCoursebyId", Name = nameof(GetCoursebyId))]
        public ActionResult<Course> GetCoursebyId(string id)
        {
            if(id != null && id != "")
            {
                var course = _courseService.GetById(id);
                if (course != null)
                {
                    return Ok(course);
                }
                else
                {
                    logger.LogError("Cannot get access to course collection in Db");
                }
            }
            else
                logger.LogError("Course Id is null or empty string");
            return BadRequest(null);

            
        }

        /// <summary>
        /// Create a new Course
        /// </summary>
        /// <param name="course">New Course data</param>
        /// <returns>Created course</returns>
        [HttpPost("createNewCourse", Name = nameof(CreateNewCourse))]
        public ActionResult<Course> CreateNewCourse(Course course)
        {
            logger.LogInformation("Creating a new course");
            if(course != null)
            {
                course.Status = true;
                if (!(_courseService.Create(course)))
                    return NotFound();
                return CreatedAtRoute("getCoursebyId", new { id = course.Id }, course);
            }
            else
            {
                logger.LogError("Course object is null " + course);
                return BadRequest();
            }

        }

        /// <summary>
        /// Update existed course profile
        /// </summary>
        /// <param name="courseIn"> Course to update</param>
        /// <returns>Boolean if Course updated true, otherwise false</returns>
        [HttpPut("updateCourse", Name = nameof(UpdateCourse))]
        public IActionResult UpdateCourse(Course courseIn)
        {
            logger.LogInformation("Updating existed course: " + courseIn.Id);
            if(courseIn != null)
            {
                var course = _courseService.GetById(courseIn.Id);
                if (course != null)
                {
                    if (_courseService.Update(courseIn.Id, courseIn))
                    {
                        logger.LogInformation("Given Course profile Updated successfully");
                        return Ok(true);
                    }
                    else
                        logger.LogError("Cannot update the Course profile: " + courseIn.Id + " wrong format");
                }
                else
                    logger.LogError("Course with Id: " + courseIn.Id + " doesn't exist");
            }
            else
                logger.LogError("CourseIn objest is null");
            return BadRequest(false);

        }

        /// <summary>
        /// Delete Course by Id
        /// </summary>
        /// <param name="id"> Course id</param>
        /// <returns>True if course deleted, otherwise false</returns>
        [HttpDelete("deleteCourseById", Name = nameof(DeleteCourseById))]
        public IActionResult DeleteCourseById(string id)
        {
            logger.LogInformation("Deleting Course by Id " + id);
            if(id != null)
            {
                var course = _courseService.GetById(id);
                if (course != null && _courseService.RemoveById(course.Id))
                    return Ok(true);
                else
                    logger.LogError("Cannot get access to admin collection in Db");
            }
            else
                logger.LogError("Id is not valid format or null");
           return NotFound(false);
 
        }

    }
}
