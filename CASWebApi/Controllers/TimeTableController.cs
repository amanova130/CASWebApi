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
        public TimeTableController(ITimeTableService timeTableService, ILogger<TimeTableController> logger)
        {
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
            var timeList = _timeTableService.GetAll();
            if (timeList != null)
            {
                logger.LogInformation("Fetched all data");
                return timeList;
            }
            else
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
                var timeTable = _timeTableService.GetById(id);
                if (timeTable == null)
                {
                    return NotFound();
                }
                else
                {
                    logger.LogInformation("Got timeTable by Id" + timeTable);
                    return Ok(timeTable);
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
                var timeTable = _timeTableService.GetByCalendarName(id);
                if (timeTable == null)
                {
                    return NotFound();
                }
                else
                {
                    logger.LogInformation("Got timeTable by Id" + timeTable);
                    return Ok(timeTable);
                }
            }
            else
            {
                logger.LogError("Id is null or empty string");
                return NotFound();
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
          bool res=_timeTableService.Create(timeTable);
            if(res)
                return CreatedAtRoute("getTTById", new { id = timeTable.Id }, timeTable);
                
            return NotFound("cannot create timeTable");
      
        }

        [HttpPut("updateById", Name = nameof(UpdateById))]
        public IActionResult UpdateById(string id, TimeTable timeTableIn)
        {
            logger.LogInformation("Updating TimeTable ");
            if (id != null && timeTableIn != null)
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
            else
                logger.LogError("id and timeTableIn are null");
            return BadRequest(null);
        }

        [HttpDelete("deleteTTById", Name = nameof(DeleteTTById))]
        public IActionResult DeleteTTById(string id)
        {
            logger.LogInformation("DEleting Time table by Id");
            if (id != null)
            {
                var timeTable = _timeTableService.GetById(id);
                if (timeTable != null && _timeTableService.RemoveById(timeTable.Id))
                {
                    logger.LogInformation("Deleted successfully");
                    return Ok(true);
                }
                    
            }
            else
                logger.LogError("Id is null");
            return BadRequest(false);
        }

    }
}

