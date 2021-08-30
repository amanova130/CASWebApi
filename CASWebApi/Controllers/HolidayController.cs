using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly ILogger logger;
        IHolidayService _holidayService;
        public HolidayController(IHolidayService holidayService, ILogger<HolidayController> logger)
        {
            this.logger = logger;
            _holidayService = holidayService;
        }

        /// <summary>
        /// Get All existed Holidays
        /// </summary>
        /// <returns>List of Holidays</returns>
        [HttpGet("getAllHolidays", Name = nameof(GetAllHolidays))]
        public ActionResult<List<Holiday>> GetAllHolidays()
        {
            logger.LogInformation("Getting all Holidays from HolidayController");
            try
            {
                var holidayList = _holidayService.GetAll();
                    logger.LogInformation("Fetched All holiday data");
                    return holidayList;                    
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// Get Holiday by Id
        /// </summary>
        /// <param name="id">Id of Holiday</param>
        /// <returns>Holiday profile</returns>
        [HttpGet("getHolidayById", Name = nameof(GetHolidayById))]
        public ActionResult<Holiday> GetHolidayById(string id)
        {
            logger.LogInformation("Getting holiday by given Id from HolidayController");
            if (id == null || id == "")
            {
                logger.LogError("Holiday Id is null or empty string");
                return BadRequest("Incorrect format of Id param");
            }
            try
            {
                var holiday = _holidayService.GetById(id);
                if (holiday != null)
                {
                    logger.LogInformation("Fetched holiday data by id");
                    return Ok(holiday);
                }
                return NotFound("holiday with given id doesn't exists");                    
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }


        }

        /// <summary>
        /// Create a new Holiday
        /// </summary>
        /// <param name="holiday">Holiday object</param>
        /// <returns>Created Holiday object</returns>
        [HttpPost("createHoliday", Name = nameof(CreateHoliday))]
        public ActionResult<Holiday> CreateHoliday(Holiday holiday)
        {
            logger.LogInformation("Creating a new holiday profile: " + holiday);
            if (holiday == null)
            {
                logger.LogError("Holiday Id is null or empty string");
                return BadRequest("Incorrect format of Id param");
            }
                holiday.Status = true;
            try
            {
                _holidayService.Create(holiday);
                  logger.LogInformation("A new holiday profile added successfully " + holiday);
                   return CreatedAtRoute("getHolidayById", new { id = holiday.Id }, holiday);               
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }

        }

        /// <summary>
        /// Update Holiday profile
        /// </summary>
        /// <param name="holidayIn">Holiday object to update</param>
        /// <returns>True if updated, otherwise false</returns>
        [HttpPut("updateHoliday", Name = nameof(UpdateHoliday))]
        public IActionResult UpdateHoliday(Holiday holidayIn)
        {
            logger.LogInformation("Updating existed Holiday profile: " + holidayIn.Id);
            if (holidayIn == null)
            {
                logger.LogError("Holiday object is null or empty string");
                return BadRequest("Incorrect format of holiday param");
            }
            try
            {
                var holiday = _holidayService.GetById(holidayIn.Id);
                if (holiday != null)
                {
                    _holidayService.Update(holidayIn.Id, holidayIn);
                    logger.LogInformation("Given holiday profile Updated successfully");
                    return Ok(true);
                }
               
                    logger.LogError("holiday with Id: " + holidayIn.Id + " doesn't exist");
                    return NotFound("Holiday with given id doesn't exists");         
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }


        }

        /// <summary>
        /// Delete Holiday by Id
        /// </summary>
        /// <param name="id">Id of Holiday</param>
        /// <returns>True if deleted, otherwise false</returns>
        [HttpDelete("deleteHolidayById", Name = nameof(DeleteHolidayById))]
        public IActionResult DeleteHolidayById(string id)
        {
            logger.LogInformation("Deleting existed holiday profile: " + id);
            if (id == null)
            {
                logger.LogError("Holiday object is null or empty string");
                return BadRequest("Incorrect format of holiday param");
            }
            try
            { 
                var holiday = _holidayService.GetById(id);
                if (holiday != null && _holidayService.RemoveById(holiday.Id))
                    return Ok(true);
                logger.LogError("Cannot get access to holiday collection in Db");
                return NotFound("holiday object with given id doesn't exists");     
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }


        }
    }
}
