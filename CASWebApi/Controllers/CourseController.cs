using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;



namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("getAllCourse", Name = nameof(GetAllCourse))]
        public ActionResult<List<Course>> GetAllCourse() =>
             _courseService.GetAll();

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
        public IActionResult UpdateCourse(string id, Course courseIn)
        {
            var course = _courseService.GetById(id);

            if (course == null)
            {
                return NotFound();
            }
            courseIn.Id = id;

            _courseService.Update(id, courseIn);

            return NoContent();
        }

        [HttpDelete("deleteCourseById", Name = nameof(DeleteCourseById))]
        public IActionResult DeleteCourseById(string id)
        {
            var course = _courseService.GetById(id);

            if (course != null && _courseService.RemoveById(course.Id))
                return NoContent();
            return NotFound();
        }
    }
}
