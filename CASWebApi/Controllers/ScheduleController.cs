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
        ITimeTableService _timeTableService;
        public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger, ITimeTableService timeTableService)
        {
            this.logger = logger;
            _scheduleService = scheduleService;
            _timeTableService = timeTableService;
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
            if (groupId != null && id != null)
            {
                var timeTable = _timeTableService.GetById(groupId);
                if (timeTable != null)
                {
                    for (int i = 0; i < timeTable.GroupSchedule.Length; i++)
                    {
                        if (timeTable.GroupSchedule[i].EventId == id)
                            return timeTable.GroupSchedule[i];
                    }
                }
                else
                    logger.LogError("Time table doesn't exist");
            }
            else
                logger.LogError("GoupId and Id are null");
            return BadRequest(null);
           
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
            if(groupId != null && newEvent != null)
            {
                CalendarService.CreateEvent(newEvent, groupId);
                if (_scheduleService.Create(groupId, newEvent))
                    return CreatedAtRoute("createEvent", new { id = newEvent.Title }, newEvent);
                else
                    logger.LogError("Faied to create a new event");
            }
            else
                logger.LogError("GroupId and newEvent  objects are null ");
            return BadRequest(null);
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
            if (groupId != null && eventIn != null)
            {
                var timeTable = _timeTableService.GetByCalendarName(groupId);
                if (timeTable != null)
                {
                    for (int i = 0; i < timeTable.GroupSchedule.Length; i++)
                    {
                        if (timeTable.GroupSchedule[i].EventId == eventIn.EventId)
                        {
                            timeTable.GroupSchedule[i] = eventIn;
                            break;
                        }
                    }
                    if (_timeTableService.Update(groupId, timeTable) && CalendarService.UpdateEvent(eventIn, groupId) != null)
                        return CreatedAtRoute("createEvent", new { id = eventIn.Title }, eventIn);
                }
                else
                    logger.LogError("Cannot access to _timeTableService.GetByCalendarName");
            }
            else
                logger.LogError("groupId and eventIn null");
            return BadRequest(null);
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
            if(eventId != null && groupId != null)
            {
                if (CalendarService.DeleteEvent(groupId, eventId))
                {
                    if (_scheduleService.RemoveById(eventId, groupId))
                        return Ok(true);
                    else
                        logger.LogError("Cannot get access to Event collection in Db");
                }
                else
                    logger.LogError("Cannot get access to Event collection in Db");
            }
            else
                logger.LogError("eventId and groupId are null");
            return BadRequest(null);

        }
    }
}
