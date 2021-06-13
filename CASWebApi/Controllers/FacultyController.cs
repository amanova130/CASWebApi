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
    public class FacultyController : ControllerBase
    {
        IFacultyService _facultyService;
        public FacultyController(IFacultyService facultiesService)
        {
            _facultyService = facultiesService;
        }

        [HttpGet("getAllFaculties", Name = nameof(GetAllFaculties))]
        public ActionResult<List<Faculty>> GetAllFaculties() =>
             _facultyService.GetAll();

        [HttpGet("getFacById", Name = nameof(GetFacById))]
        public ActionResult<Faculty> GetFacById(string id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty == null)
            {
                return NotFound();
            }

            return faculty;
        }

        [HttpPost("createFaculty", Name = nameof(CreateFaculty))]
        public ActionResult<Faculty> CreateFaculty(Faculty faculty)
        {
            if(!(_facultyService.Create(faculty)))
                return NotFound();

            return CreatedAtRoute("getFacById", new { id = faculty.Id }, faculty);
        }

        [HttpPut("updateFaculty", Name = nameof(UpdateFaculty))]
        public IActionResult UpdateFaculty(string id, Faculty facultyIn)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty == null)
            {
                return NotFound();
            }
            facultyIn.Id = id;

            _facultyService.Update(id, facultyIn);

            return NoContent();
        }

        [HttpDelete("deleteFacById", Name = nameof(DeleteFacById))]
        public IActionResult DeleteFacById(string id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty != null && _facultyService.RemoveById(faculty.Id))
                return NoContent();
            return NotFound();
        }
    }
}
