using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ILogger logger;
        ICourseService _courseService;
        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            this.logger = logger;
            _courseService = courseService;
        }

        [HttpGet("getAllCourse", Name = nameof(GetAllCourse))]
        public ActionResult<List<Course>> GetAllCourse() =>
             _courseService.GetAll();

        [HttpGet("getNumberOfCourses", Name = nameof(getNumberOfCourses))]
        public ActionResult<int> getNumberOfCourses() =>
             _courseService.GetNumberOfCourses();

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

        [HttpPost("createNewCourse", Name = nameof(CreateNewCourse))]
        public ActionResult<Course> CreateNewCourse(Course course)
        {
            if(!(_courseService.Create(course)))
                return NotFound();

            return CreatedAtRoute("getCoursebyId", new { id = course.Id }, course);
        }

        [HttpPut("updateCourse", Name = nameof(UpdateCourse))]
        public IActionResult UpdateCourse(Course courseIn)
        {
            logger.LogInformation("Updating existed teacher profile: " + courseIn.Id);
            var teacher = _courseService.GetById(courseIn.Id);

            if (teacher == null)
            {
                logger.LogError("Teacher with Id: " + courseIn.Id + " doesn't exist");
                return NotFound(false);
            }
            bool updated = _courseService.Update(courseIn.Id, courseIn);
            if (updated)
                logger.LogInformation("Given Teacher profile Updated successfully");
            else
                logger.LogError("Cannot update the teacher profile: " + courseIn.Id + " wrong format");

            return Ok(updated);
        }

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
