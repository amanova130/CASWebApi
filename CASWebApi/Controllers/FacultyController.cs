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

        [HttpGet]
        public ActionResult<List<Faculty>> Get() =>
             _facultyService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetFaculty")]
        public ActionResult<Faculty> Get(string id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty == null)
            {
                return NotFound();
            }

            return faculty;
        }

        [HttpPost]
        public ActionResult<Faculty> Create(Faculty faculty)
        {
            if(!(_facultyService.Create(faculty)))
                return NotFound();

            return CreatedAtRoute("GetFaculty", new { id = faculty.Id }, faculty);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Faculty facultyIn)
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

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty != null && _facultyService.RemoveById(faculty.Id))
                return NoContent();
            return NotFound();
        }
    }
}
