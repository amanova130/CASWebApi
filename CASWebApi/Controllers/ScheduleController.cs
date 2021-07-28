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
    public class ScheduleController : ControllerBase
    {
        IScheduleService _scheduleService;
        ITimeTableService _timeTableService;
        public ScheduleController(IScheduleService scheduleService, ITimeTableService timeTableService)
        {
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
            var timeTable = _timeTableService.GetById(groupId);
            

            if (timeTable == null)
            {
                return NotFound();
            }

            for (int i = 0; i < timeTable.GroupSchedule.Length; i++)
            {
                if(timeTable.GroupSchedule[i].EventId == id)
                    return timeTable.GroupSchedule[i];
            }
            return NotFound();
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
            CalendarService.CreateEvent(newEvent,groupId);

            if (!(_scheduleService.Create(groupId, newEvent)))
                return NotFound();

            return CreatedAtRoute("createEvent", new { id = newEvent.Title }, newEvent);
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

            var timeTable = _timeTableService.GetByCalendarName(groupId);

            for (int i = 0; i < timeTable.GroupSchedule.Length; i++)
            {
                if (timeTable.GroupSchedule[i].EventId == eventIn.EventId)
                {
                    timeTable.GroupSchedule[i] = eventIn;
                    break;
                }

            }

            if(_timeTableService.Update(groupId, timeTable)  && CalendarService.UpdateEvent(eventIn,groupId) != null)
                return CreatedAtRoute("createEvent", new { id = eventIn.Title }, eventIn);
            return NotFound();
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
            if (CalendarService.DeleteEvent(groupId, eventId))
            {

                if (_scheduleService.RemoveById(eventId, groupId))
                return Ok(true);
            }
            return NotFound(false);

        }
    }
}
