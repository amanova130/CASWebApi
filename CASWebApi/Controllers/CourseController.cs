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
        public ActionResult<List<Course>> GetAllCourse() =>
             _courseService.GetAll();

        /// <summary>
        /// Get Number of Courses
        /// </summary>
        /// <returns>Number of Courses</returns>
        [HttpGet("getNumberOfCourses", Name = nameof(getNumberOfCourses))]
        public ActionResult<int> getNumberOfCourses() =>
             _courseService.GetNumberOfCourses();

        /// <summary>
        /// Get Course profile by Id
        /// </summary>
        /// <param name="id">Id of course</param>
        /// <returns>Course profile</returns>
        [HttpGet("getCoursebyId", Name = nameof(GetCoursebyId))]
        public ActionResult<Course> GetCoursebyId(string id)
        {
            var course = _courseService.GetById(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        /// <summary>
        /// Create a new Course
        /// </summary>
        /// <param name="course">New Course data</param>
        /// <returns>Created course</returns>
        [HttpPost("createNewCourse", Name = nameof(CreateNewCourse))]
        public ActionResult<Course> CreateNewCourse(Course course)
        {
            course.Status = true;
            if(!(_courseService.Create(course)))
                return NotFound();

            return CreatedAtRoute("getCoursebyId", new { id = course.Id }, course);
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
            var course = _courseService.GetById(courseIn.Id);

            if (course == null)
            {
                logger.LogError("Course with Id: " + courseIn.Id + " doesn't exist");
                return NotFound(false);
            }
            bool updated = _courseService.Update(courseIn.Id, courseIn);
            if (updated)
                logger.LogInformation("Given Course profile Updated successfully");
            else
                logger.LogError("Cannot update the Course profile: " + courseIn.Id + " wrong format");

            return Ok(updated);
        }

        /// <summary>
        /// Delete Course by Id
        /// </summary>
        /// <param name="id"> Course id</param>
        /// <returns>True if course deleted, otherwise false</returns>
        [HttpDelete("deleteCourseById", Name = nameof(DeleteCourseById))]
        public IActionResult DeleteCourseById(string id)
        {
            var course = _courseService.GetById(id);

            if (course != null && _courseService.RemoveById(course.Id))
                return Ok(true);
            return NotFound(false);
        }

    }
}
