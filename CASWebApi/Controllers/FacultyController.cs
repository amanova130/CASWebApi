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
            try
            {
                var facultyList = _facultyService.GetAll();
                logger.LogInformation("Fetched all data");
                return facultyList; 
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
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
            try
            {
                return _facultyService.GetNumberOfFaculties();
                logger.LogInformation("Fetched all data");

            }
            catch(Exception e)
            {
                logger.LogError("Cannot get access to Db");
                return BadRequest("No connection to database");
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
            if (id == null || id == "")
            {
                logger.LogError("Course Id is null or empty string");
                return BadRequest("incorrect format of id parameter");
            }
            try
            {
                var faculty = _facultyService.GetById(id);
                if (faculty != null)
                    return Ok(faculty);             
                    logger.LogError("faculty with given id doesn't exists in database");
                return NotFound("faculty with given id doesn't exists in database");
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
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
            if (faculty == null)
            {
                logger.LogError("Faculty object is null ");
                return BadRequest("incorrect faculty model sent");
            }
            faculty.Status = true;
            try
            {
                _facultyService.Create(faculty);
                    return CreatedAtRoute("getFacById", new { id = faculty.Id }, faculty);           
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
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
            if (facultyIn == null || facultyIn.Id == null)
            {
                logger.LogError("CourseIn objest or his id is null");
                return BadRequest("incorrect model of faculty sent");
            }
            try
            {
                var faculty = _facultyService.GetById(facultyIn.Id);

                if (faculty != null)
                {
                    _facultyService.Update(facultyIn);
                    logger.LogInformation("Given Faculty profile Updated successfully");
                    return Ok(true);
                }
                else
                    logger.LogError("Faculty with Id: " + facultyIn.Id + " doesn't exist");
                return NotFound("faculty with given id doesn't exists");
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
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
            if (id == null)
            {
                logger.LogError("Id is not valid format or null");
                return NotFound(false);
            }
            try
            {
                var faculty = _facultyService.GetById(id);
                if (faculty == null )
                {
                    logger.LogError("faculty with given id doesn't exists");
                    return NotFound("faculty with given id doesn't exists");
                }
                _facultyService.RemoveById(faculty.Id);
                 return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }


        }
    }
}
