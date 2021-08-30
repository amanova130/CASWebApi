using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        //Logger to create streammer of logs 
        private readonly ILogger logger;
        ITimeTableService _timeTableService;
        IStudentService _studentService;
        public TimeTableController(ITimeTableService timeTableService, IStudentService studentService,ILogger<TimeTableController> logger)
        {
            _studentService=studentService;
            this.logger = logger;
            _timeTableService = timeTableService;
        }

        /// <summary>
        /// get all timetables from db
        /// </summary>
        /// <returns>list of timeTables</returns>
        [HttpGet("getAllTTable", Name = nameof(GetAllTTable))]
        public ActionResult<List<TimeTable>> GetAllTTable()
        {
            logger.LogInformation("Getting all TimeTables data");
            try
            {
                var timeList = _timeTableService.GetAll();
                logger.LogInformation("Fetched all data");
                return timeList;
            }
           catch(Exception e)
            {
                logger.LogError("Cannot get access to Time-table collection in Db");
                return StatusCode(500, "Internal server error");
            }
        }


        /// <summary>
        /// get single time table by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>found "timeTable" object,or "NotFound" if not found</returns>
        [HttpGet("getTTById", Name = nameof(GetTTById))]
        public ActionResult<TimeTable> GetTTById(string id)
        {
            logger.LogInformation("Getting TimeTable data");
            if (id != null && id != "")
            {
                try
                {
                    var timeTable = _timeTableService.GetById(id);
                    if (timeTable == null)
                    {
                        return NotFound("Time Table not found");
                    }
                    else
                    {
                        logger.LogInformation("Got timeTable by Id" + timeTable);
                        return Ok(timeTable);
                    }
                }
               catch(Exception e)
                {
                    logger.LogError("Cannot get access to Db");
                    return StatusCode(500, "Internal server error");
                }
            }
            else
            {
                logger.LogError("Id is null or empty string");
                return NotFound();
            }
        }

      
        /// <summary>
        /// get single time table by group id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>found "timeTable" object,or "NotFound" if not found</returns>
        [HttpGet("getTTByGroup", Name = nameof(GetTTByGroup))]
        public ActionResult<TimeTable> GetTTByGroup(string id)
        {
            logger.LogInformation("Getting time table by group id");
            if(id != null)
            {
                try
                {
                    var timeTable = _timeTableService.GetByCalendarName(id);
                    if (timeTable == null)
                    {
                        return NotFound("Time table not found");
                    }
                    else
                    {
                        logger.LogInformation("Got timeTable by Id" + timeTable);
                        return Ok(timeTable);
                    }
                }
                catch(Exception e)
                {
                    logger.LogError("Cannot get access to Db");
                    return StatusCode(500, "Internal server error");
                } 
            }
            else
            {
                logger.LogError("Id is null or empty string");
                return BadRequest("Id is null or empty string");
            }
        }

        /// <summary>
        /// add new "timeTable" object to db
        /// </summary>
        /// <param name="timeTable"></param>
        /// <returns></returns>
        [HttpPost("createTimeTable", Name = nameof(CreateTimeTable))]
        public ActionResult<TimeTable> CreateTimeTable(TimeTable timeTable)
        {
            if (timeTable != null)
            {
                try
                {
                    _timeTableService.Create(timeTable);
                    return CreatedAtRoute("getTTById", new { id = timeTable.Id }, timeTable);
                }
                catch (Exception e)
                {
                    return Conflict(e);
                }
            }
            else
            {
                logger.LogError("A new timetable model is not valid");
                return BadRequest("A new timetable model is not valid");
            }
        }

        /// <summary>
        /// Update Time table by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeTableIn"></param>
        /// <returns></returns>
        [HttpPut("updateById", Name = nameof(UpdateById))]
        public IActionResult UpdateById(string id, TimeTable timeTableIn)
        {
            logger.LogInformation("Updating TimeTable ");
            if (id != null && timeTableIn != null)
            {
                try
                {
                    var timeTable = _timeTableService.GetById(id);
                    if (timeTable != null)
                    {
                        timeTableIn.Id = id;
                        _timeTableService.Update(id, timeTableIn);
                        logger.LogInformation("Timetable is updated");
                        return Ok(true);
                    }
                    else
                        logger.LogError("Failed to get Timetable by Id");
                }
                catch(Exception e)
                {
                    return BadRequest("No connection to database");
                }
               
            }
            else
                logger.LogError("id and timeTableIn are null");
            return BadRequest("id and timeTableIn are null");
        }

        /// <summary>
        /// Delete Time table by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Result or error if timetable object is not valid</returns>
        [HttpDelete("deleteTTById", Name = nameof(DeleteTTById))]
        public IActionResult DeleteTTById(string id)
        {
            logger.LogInformation("DEleting Time table by Id");
            if (id != null)
            {
                try
                {
                    var timeTable = _timeTableService.GetById(id);
                    if (timeTable != null && _timeTableService.RemoveById(timeTable.Id))
                    {
                        logger.LogInformation("Deleted successfully");
                        return Ok(true);
                    }
                }
                catch(Exception e)
                {
                    return BadRequest("No connection to database");
                }
            } 
            else
                logger.LogError("Id is null");
            return NotFound("Id is not valid or empty string");
        }

    }
}

