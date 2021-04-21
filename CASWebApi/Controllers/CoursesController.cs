using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public ActionResult<List<Course>> Get() =>
             _courseService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetCourse")]
        public ActionResult<Course> Get(string id)
        {
            var course = _courseService.GetById(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        [HttpPost]
        public ActionResult<Course> Create(Course course)
        {
            _courseService.Create(course);

            return CreatedAtRoute("GetCourse", new { id = course.Id }, course);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Course courseIn)
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

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var course = _courseService.GetById(id);

            if (course == null)
            {
                return NotFound();
            }

            _courseService.RemoveById(course.Id);

            return NoContent();
        }
    }
}
