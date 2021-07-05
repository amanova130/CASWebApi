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
            course.Status = true;
            if(!(_courseService.Create(course)))
                return NotFound();

            return CreatedAtRoute("getCoursebyId", new { id = course.Id }, course);
        }

        [HttpPut("updateCourse", Name = nameof(UpdateCourse))]
        public IActionResult UpdateCourse(Course courseIn)
        {
            logger.LogInformation("Updating existed course: " + courseIn.Id);
            var teacher = _courseService.GetById(courseIn.Id);

            if (teacher == null)
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

        [HttpDelete("deleteCourseById", Name = nameof(DeleteCourseById))]
        public IActionResult DeleteCourseById(string id)
        {
            var course = _courseService.GetById(id);

            if (course != null && _courseService.RemoveById(course.Id))
                return Ok(true);
            return NotFound(false);
        }

        [HttpPost("uploadImage", Name = nameof(UploadImage)), DisableRequestSizeLimit]
        public IActionResult UploadImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
