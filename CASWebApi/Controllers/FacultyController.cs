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
    public class FacultyController : ControllerBase
    {
        IFacultyService _facultyService;
        public FacultyController(IFacultyService facultiesService)
        {
            _facultyService = facultiesService;
        }

        /// <summary>
        /// Get All Faculties data
        /// </summary>
        /// <returns>List of Faculties</returns>
        [HttpGet("getAllFaculties", Name = nameof(GetAllFaculties))]
        public ActionResult<List<Faculty>> GetAllFaculties() =>
             _facultyService.GetAll();

        /// <summary>
        /// Get Number of existed faculties
        /// </summary>
        /// <returns>Number of faculties</returns>
        [HttpGet("getNumberOfFaculties", Name = nameof(GetNumberOfFaculties))]
        public ActionResult<long> GetNumberOfFaculties() =>
             _facultyService.GetNumberOfFaculties();

        /// <summary>
        /// Get Faculty profile by Id
        /// </summary>
        /// <param name="id">Id of faculty</param>
        /// <returns>Faculty profile</returns>
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

        /// <summary>
        /// Create a new Faculty
        /// </summary>
        /// <param name="faculty">New Faculty Object</param>
        /// <returns>Created Faculty profile</returns>
        [HttpPost("createFaculty", Name = nameof(CreateFaculty))]
        public ActionResult<Faculty> CreateFaculty(Faculty faculty)
        {
            if(!(_facultyService.Create(faculty)))
                return NotFound("duplicated id or wrong id format");

            return CreatedAtRoute("getFacById", new { id = faculty.Id }, faculty);
        }

        /// <summary>
        /// Update existed faculty profile
        /// </summary>
        /// <param name="facultyIn">Faculty profile to update</param>
        /// <returns>Updated Faculty</returns>
        [HttpPut("updateFaculty", Name = nameof(UpdateFaculty))]
        public IActionResult UpdateFaculty(Faculty facultyIn)
        {
            var faculty = _facultyService.GetById(facultyIn.Id);

            if (faculty == null)
            {
                return NotFound();
            }
            bool updated = _facultyService.Update(facultyIn.Id, facultyIn);

            return Ok(updated);
        }

        /// <summary>
        /// Delete Faculty by Id
        /// </summary>
        /// <param name="id">Id of Faculty</param>
        /// <returns>True if faculty dleted, otherwise false</returns>
        [HttpDelete("deleteFacById", Name = nameof(DeleteFacById))]
        public IActionResult DeleteFacById(string id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty != null && _facultyService.RemoveById(faculty.Id))
                return Ok(true);
            return NotFound(false);
        }
    }
}
