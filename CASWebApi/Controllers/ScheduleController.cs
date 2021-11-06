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
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger logger;
        IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger)
        {
            this.logger = logger;
            _scheduleService = scheduleService;
        }
        //[HttpGet]
        //public ActionResult<List<Schedule>> Get() =>
        //     _scheduleService.GetAll();

        /// <summary>
        /// get schedule by id and groupId(calendar's name)
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="id"></param>
        /// <returns>found schedule object,or not found</returns>

        [HttpGet("getEvent", Name = nameof(getEvent))]
        public ActionResult<Schedule> getEvent(string groupId,string id)
        {
            logger.LogInformation("Getting Event by group id and id of event");
            if (id == null || groupId == null)
            {
                logger.LogError("groupId or id is null or empty string");
                return BadRequest("Incorrect format of id or groupId");
            }
            try
            { 
            var schedule = _scheduleService.GetEvent(groupId, id);
                if (schedule != null)
                    return schedule;                            
                    logger.LogError("schedule doesn't exist");
                return NotFound("event with given id doesn't exists");
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }

        }

        /// <summary>
        /// create a new event for specific group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="newEvent"></param>
        /// <returns>created event if added successfully,"not found" otherwise</returns>
        [HttpPost("createEvent", Name = nameof(createEvent))]
        public ActionResult<Schedule> createEvent(string groupId,Schedule newEvent)
        {
            logger.LogInformation("Creating a new Event for calendar");
            if (newEvent == null || groupId == null)
            {
                logger.LogError("groupId or newEvent is null");
                return BadRequest("Incorrect format of groupId or newEvent");
            }
            try
            {
                _scheduleService.Create(groupId, newEvent);
                    return CreatedAtRoute("createEvent", new { id = newEvent.Title }, newEvent);   
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// update existed event
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="eventIn"></param>
        /// <returns>updated object if updated,false otherwise</returns>

        [HttpPut("updateEvent", Name = nameof(UpdateEvent))]
        public IActionResult UpdateEvent(string groupId, Schedule eventIn)
        {
            logger.LogInformation("Updating a event");
            if (eventIn == null || groupId == null)
            {
                logger.LogError("groupId or eventIn is null");
                return BadRequest("Incorrect format of groupId or eventIn");
            }
            try
            {
                if (_scheduleService.Update(groupId, eventIn))
                {
                    logger.LogInformation("event updated successfully");
                    return CreatedAtRoute("createEvent", new { id = eventIn.Title }, eventIn);
                }
                else
                {
                    logger.LogError("Faied to update an event");
                    return NotFound("event with given id doesn't exists");
                }
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// delete event by id for specific group
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="groupId"></param>
        /// <returns>true if deleted,false otherwise</returns>
        [HttpDelete("deleteEvent", Name = nameof(DeleteEvent))]
        public IActionResult DeleteEvent(string eventId, string groupId)
        {
            logger.LogInformation("Deleting a event");
            if (eventId == null || groupId == null)
            {
                logger.LogError("groupId or eventIn is null");
                return BadRequest("Incorrect format of groupId or eventIn");
            }
            try
            {
                if(_scheduleService.RemoveById(eventId, groupId))
                    return Ok(true);
                else
                    logger.LogError("event with given id not found");
                return NotFound("event with given id not found");
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }
           

        
    }
}
