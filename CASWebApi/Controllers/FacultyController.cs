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
    public class FacultyController : ControllerBase
    {
        private readonly ILogger logger;
        IFacultyService _facultyService;
        public FacultyController(IFacultyService facultiesService, ILogger<FacultyController> logger)
        {
            this.logger = logger;
            _facultyService = facultiesService;
        }

        /// <summary>
        /// Get All Faculties data
        /// </summary>
        /// <returns>List of Faculties</returns>
        [HttpGet("getAllFaculties", Name = nameof(GetAllFaculties))]
        public ActionResult<List<Faculty>> GetAllFaculties()
        {
            logger.LogInformation("Getting all Faculties data");
            var facultyList = _facultyService.GetAll();
            if(facultyList != null)
            {
                logger.LogInformation("Fetched all data");
                return facultyList;
            }
            else
            {
                logger.LogError("Cannot get access to Faculty collection in Db");
                return StatusCode(500, "Internal server error");
            }
        }
            

        /// <summary>
        /// Get Number of existed faculties
        /// </summary>
        /// <returns>Number of faculties</returns>
        [HttpGet("getNumberOfFaculties", Name = nameof(GetNumberOfFaculties))]
        public ActionResult<long> GetNumberOfFaculties()
        {
            logger.LogInformation("Getting number of Faculties");
            var numberOfFaculties = _facultyService.GetNumberOfFaculties();
            if (numberOfFaculties > 0)
            {
                return Ok(numberOfFaculties);
            }
            else
            {
                logger.LogError("Cannot get access to Faculty collection in Db");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get Faculty profile by Id
        /// </summary>
        /// <param name="id">Id of faculty</param>
        /// <returns>Faculty profile</returns>
        [HttpGet("getFacById", Name = nameof(GetFacById))]
        public ActionResult<Faculty> GetFacById(string id)
        {
            logger.LogInformation("Getting Faculty by Id");
            if(id != null)
            {
                var faculty = _facultyService.GetById(id);
                if (faculty != null)
                {
                    return Ok(faculty);
                }
                else
                {
                    logger.LogError("Cannot get access to faculty collection in Db");
                }
            }
            else
                logger.LogError("Course Id is null or empty string");
            return BadRequest(null);
        }

        [HttpGet("getAvgOfFacultiesByCourse", Name = nameof(GetAvgOfFacultiesByCourse))]
        public ActionResult<List<object>> GetAvgOfFacultiesByCourse(string courseName,string facId)
        {
            logger.LogInformation("Getting Faculty by Id");
            if (courseName != null && facId != null)
            {
                var faculties = _facultyService.GetAvgOfFacultiesByCourse(courseName,facId);
                if (faculties != null)
                {
                    return Ok(faculties);
                }
                else
                {
                    logger.LogError("Cannot get access to faculty collection in Db");
                }
            }
            else
                logger.LogError("Course Id is null or empty string");
            return BadRequest(null);
        }

        /// <summary>
        /// Create a new Faculty
        /// </summary>
        /// <param name="faculty">New Faculty Object</param>
        /// <returns>Created Faculty profile</returns>
        [HttpPost("createFaculty", Name = nameof(CreateFaculty))]
        public ActionResult<Faculty> CreateFaculty(Faculty faculty)
        {
            logger.LogInformation("Creating a new faculty");
            if(faculty != null)
            {
                faculty.Status = true;
                if (_facultyService.Create(faculty))
                    return CreatedAtRoute("getFacById", new { id = faculty.Id }, faculty);
                else
                    return StatusCode(409, "Duplicated Id");
            }
            else
                logger.LogError("Faculty object is null " + faculty);
            return BadRequest(null);
        }

        /// <summary>
        /// Update existed faculty profile
        /// </summary>
        /// <param name="facultyIn">Faculty profile to update</param>
        /// <returns>Updated Faculty</returns>
        [HttpPut("updateFaculty", Name = nameof(UpdateFaculty))]
        public IActionResult UpdateFaculty(Faculty facultyIn)
        {
            logger.LogInformation("Updating existed faculty: " + facultyIn.Id);
            if(facultyIn != null)
            {
                var faculty = _facultyService.GetById(facultyIn.Id);

                if (faculty != null)
                {
                    if(_facultyService.Update(facultyIn.Id, facultyIn))
                    {
                        logger.LogInformation("Given Faculty profile Updated successfully");
                        return Ok(true);
                    }
                   else
                        logger.LogError("Cannot update the Faculty profile: " + facultyIn.Id + " wrong format");
                }
                else
                    logger.LogError("Faculty with Id: " + facultyIn.Id + " doesn't exist");

            }
            else
                logger.LogError("CourseIn objest is null");
            return BadRequest(false);
        }

        /// <summary>
        /// Delete Faculty by Id
        /// </summary>
        /// <param name="id">Id of Faculty</param>
        /// <returns>True if faculty dleted, otherwise false</returns>
        [HttpDelete("deleteFacById", Name = nameof(DeleteFacById))]
        public IActionResult DeleteFacById(string id)
        {
            logger.LogInformation("Deleting Faculty by Id " + id);
            if (id != null)
            {
                var faculty = _facultyService.GetById(id);
                if (faculty != null && _facultyService.RemoveById(faculty.Id))
                    return Ok(true);
                else
                    logger.LogError("Cannot get access to faculty collection in Db");
            }
            else
                logger.LogError("Id is not valid format or null");
            return NotFound(false);
        }
    }
}
