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

        [HttpGet]
        public ActionResult<List<TimeTable>> Get() =>
             _timeTableService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetTimeTable")]
        public ActionResult<TimeTable> Get(string id)
        {
            var timeTable = _timeTableService.GetById(id);

            if (timeTable == null)
            {
                return NotFound();
            }

            return timeTable;
        }

        [HttpPost]
        public ActionResult<TimeTable> Create(TimeTable timeTable)
        {
            timeTable.status = true;
            if (_timeTableService.GetByCalendarName(timeTable.CalendarName) == null)
            {
                timeTable.CalendarId = Calendar.CreateCalendar(timeTable.CalendarName);
                Calendar.CreateEvent(timeTable.CalendarName, timeTable.GroupSchedule);

                _timeTableService.Create(timeTable);


            }
            else
            {
                _timeTableService.AddToSchedule(timeTable.GroupSchedule, timeTable.CalendarName);
                Calendar.CreateEvent(timeTable.CalendarName, timeTable.GroupSchedule);
                timeTable = _timeTableService.GetByCalendarName(timeTable.CalendarName);

            }

            return CreatedAtRoute("GetTimeTable", new { id = timeTable.Id }, timeTable);
        }
       
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, TimeTable timeTableIn)
        {
            var timeTable = _timeTableService.GetById(id);

            if (timeTable == null)
            {
                return NotFound();
            }
            timeTableIn.Id = id;

            _timeTableService.Update(id,timeTableIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var timeTable = _timeTableService.GetById(id);
            for(int i=0;i<timeTable.GroupSchedule.Length;i++)
            Calendar.DeleteEvent(timeTable.CalendarName,timeTable.GroupSchedule[i].Summary); //its not supposes to be here,just 4 test
            if (timeTable != null && _timeTableService.RemoveById(timeTable.Id))
                return NoContent();
            return NotFound();
        }

        /*[HttpDelete("{eventId:length(24)}")]
        public IActionResult DeleteEvent(string id)
        {
            List<TimeTable> timeTable = _timeTableService.GetAll();
            for (int i = 0; i < timeTable.Count; i++)
            {
                for (int j = 0; j < timeTable[i].GroupSchedule.Length;j++)
                {
                    if (timeTable[i].GroupSchedule[j].eventId.Equals(id))
                    {
                        _timeTableService.deleteEvent(id);
                        Calendar.DeleteEvent(timeTable[i].CalendarName, timeTable[i].GroupSchedule[j].eventId);
                        return NoContent();

                    }
                }

            }
            return NotFound();
        }*/
    }
}
