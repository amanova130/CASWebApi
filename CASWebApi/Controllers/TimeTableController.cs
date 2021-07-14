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
    public class TimeTableController : ControllerBase
    {
        ITimeTableService _timeTableService;
        public TimeTableController(ITimeTableService timeTableService)
        {
            _timeTableService = timeTableService;
        }

        [HttpGet("getAllTTable", Name = nameof(GetAllTTable))]
        public ActionResult<List<TimeTable>> GetAllTTable() =>
             _timeTableService.GetAll();

        [HttpGet("getTTById", Name = nameof(GetTTById))]
        public ActionResult<TimeTable> GetTTById(string id)
        {
            var timeTable = _timeTableService.GetById(id);

            if (timeTable == null)
            {
                return NotFound();
            }

            return timeTable;
        }
        [HttpGet("getTTByGroup", Name = nameof(GetTTByGroup))]
        public ActionResult<TimeTable> GetTTByGroup(string id)
        {
            var timeTable = _timeTableService.GetByCalendarName(id);

            if (timeTable == null)
            {
                return NotFound();
            }

            return timeTable;
        }


        [HttpPost("createTimeTable", Name = nameof(CreateTimeTable))]
        public ActionResult<TimeTable> CreateTimeTable(TimeTable timeTable)
        {
            timeTable.status = true;
            if (_timeTableService.GetByCalendarName(timeTable.CalendarName) == null)
            {
                timeTable.CalendarId = Calendar.CreateCalendar(timeTable.CalendarName);
                // Calendar.CreateEvent(timeTable.CalendarName, timeTable.GroupSchedule);

                _timeTableService.Create(timeTable);


            }
            else
            {
                /* _timeTableService.AddToSchedule(timeTable.GroupSchedule, timeTable.CalendarName);
                 Calendar.CreateEvent(timeTable.CalendarName, timeTable.GroupSchedule);
                 timeTable = _timeTableService.GetByCalendarName(timeTable.CalendarName);*/
                return NotFound("this calendar already exists");

            }

            return CreatedAtRoute("getTTById", new { id = timeTable.Id }, timeTable);
        }

        [HttpPut("updateById", Name = nameof(UpdateById))]
        public IActionResult UpdateById(string id, TimeTable timeTableIn)
        {
            var timeTable = _timeTableService.GetById(id);

            if (timeTable == null)
            {
                return NotFound();
            }
            timeTableIn.Id = id;

            _timeTableService.Update(id, timeTableIn);

            return NoContent();
        }

        [HttpDelete("deleteTTById", Name = nameof(DeleteTTById))]
        public IActionResult DeleteTTById(string id)
        {
            var timeTable = _timeTableService.GetById(id);
            for (int i = 0; i < timeTable.GroupSchedule.Length; i++)
                Calendar.DeleteEvent(timeTable.CalendarName, timeTable.GroupSchedule[i].title); //its not supposes to be here,just 4 test
            if (timeTable != null && _timeTableService.RemoveById(timeTable.Id))
                return NoContent();
            return NotFound();
        }

        //[HttpDelete("deleteEvent", Name =nameof(DeleteEvent))]
        //public IActionResult DeleteEvent(string id)
        //{
        //    List<TimeTable> timeTable = _timeTableService.GetAll();
        //    for (int i = 0; i < timeTable.Count; i++)
        //    {
        //        for (int j = 0; j < timeTable[i].GroupSchedule.Length; j++)
        //        {
        //            if (timeTable[i].GroupSchedule[j].eventId.Equals(id))
        //            {
        //                _timeTableService.deleteEvent(id);
        //                Calendar.DeleteEvent(timeTable[i].CalendarName, timeTable[i].GroupSchedule[j].eventId);
        //                return NoContent();

        //            }
        //        }

        //    }
        //    return NotFound();
        //}
    }
}

